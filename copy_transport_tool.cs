using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class CopyTransportTool
{
    static byte[] TM_SIGNATURE = new byte[] { 0xD4, 0x1E, 0xEF, 0x0D, 0x05, 0xFD, 0xFF, 0x03, 0x02, 0xD9, 0x14, 0x0D, 0xF3, 0x06, 0xFE, 0x0D };
    static byte[] DM_SIGNATURE = new byte[] { 0xC4, 0x25, 0x0A, 0x01, 0xFE, 0xF7, 0xFA, 0x11, 0xD9, 0x14, 0x0D, 0xF3, 0x06, 0xFE, 0x0D };

    public static void Main(string[] args)
    {
        string savesDir = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves";
        string srcPath = Path.Combine(savesDir, "QuangNinh backup .crp");
        // Prefer starting from clean original if available, else current backup2
        string baseTargetPath = Path.Combine(savesDir, "QuangNinh backup2.crp.bak_original");
        if (!File.Exists(baseTargetPath))
        {
            baseTargetPath = Path.Combine(savesDir, "QuangNinh backup2.crp");
        }
        string finalTargetPath = Path.Combine(savesDir, "QuangNinh backup2.crp");

        Console.WriteLine("=================================================");
        Console.WriteLine("🚀 COPY TRANSPORT LINES FROM SAVE 1 TO SAVE 2");
        Console.WriteLine("Source Save (QuangNinh backup): " + srcPath);
        Console.WriteLine("Base Target Save: " + baseTargetPath);
        Console.WriteLine("Final Output Save: " + finalTargetPath);
        Console.WriteLine("=================================================");

        // 1. Read source save and find TM chunk
        byte[] srcBytes = File.ReadAllBytes(srcPath);
        int srcTmStart = FindSignature(srcBytes, TM_SIGNATURE, 10000);
        int srcDmStart = FindSignature(srcBytes, DM_SIGNATURE, srcTmStart);
        if (srcTmStart < 0 || srcDmStart < 0 || srcDmStart <= srcTmStart)
        {
            throw new Exception(string.Format("Failed to locate TM bounds in Source: TM={0}, DM={1}", srcTmStart, srcDmStart));
        }
        int srcTmLen = srcDmStart - srcTmStart;
        Console.WriteLine(string.Format("Source TM: [{0} .. {1}) -> Length: {2} bytes", srcTmStart, srcDmStart, srcTmLen));

        byte[] cleanTmChunk = new byte[srcTmLen];
        Buffer.BlockCopy(srcBytes, srcTmStart, cleanTmChunk, 0, srcTmLen);

        // 2. Read target save and find target TM bounds
        byte[] tgtBytes = File.ReadAllBytes(baseTargetPath);
        int tgtTmStart = FindSignature(tgtBytes, TM_SIGNATURE, 10000);
        int tgtDmStart = FindSignature(tgtBytes, DM_SIGNATURE, tgtTmStart);
        if (tgtTmStart < 0 || tgtDmStart < 0 || tgtDmStart <= tgtTmStart)
        {
            throw new Exception(string.Format("Failed to locate TM bounds in Target: TM={0}, DM={1}", tgtTmStart, tgtDmStart));
        }
        int tgtTmLen = tgtDmStart - tgtTmStart;
        int delta = srcTmLen - tgtTmLen;
        Console.WriteLine(string.Format("Target TM: [{0} .. {1}) -> Length: {2} bytes", tgtTmStart, tgtDmStart, tgtTmLen));
        Console.WriteLine(string.Format("Delta: {0} bytes", delta));

        // 3. Assemble new target buffer
        int newLength = tgtBytes.Length + delta;
        byte[] newBytes = new byte[newLength];

        // Part A: Before TM
        Buffer.BlockCopy(tgtBytes, 0, newBytes, 0, tgtTmStart);

        // Part B: Clean TM from Source
        Buffer.BlockCopy(cleanTmChunk, 0, newBytes, tgtTmStart, srcTmLen);

        // Part C: Remainder after TM
        int remainderLen = tgtBytes.Length - tgtDmStart;
        Buffer.BlockCopy(tgtBytes, tgtDmStart, newBytes, tgtTmStart + srcTmLen, remainderLen);

        // 4. Update Package Headers dynamically
        UpdatePackageHeader(newBytes, delta);

        // 5. Verification
        Console.WriteLine("\n--- Verifying Output Structure ---");
        VerifyPackage(newBytes);
        int verifyTm = FindSignature(newBytes, TM_SIGNATURE, 10000);
        int verifyDm = FindSignature(newBytes, DM_SIGNATURE, verifyTm);
        Console.WriteLine(string.Format("Verified TM at {0}, DM at {1} (Chunk size = {2})", verifyTm, verifyDm, verifyDm - verifyTm));
        if (verifyDm - verifyTm != srcTmLen)
        {
            throw new Exception("Verification failed: TM chunk length does not match source!");
        }

        // 6. Create timestamped backup of target before replacing
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string backupFile = finalTargetPath + ".bak_" + timestamp;
        File.Copy(finalTargetPath, backupFile, true);
        Console.WriteLine("Created backup: " + backupFile);

        // 7. Write final save
        File.WriteAllBytes(finalTargetPath, newBytes);
        Console.WriteLine("Successfully wrote updated save to: " + finalTargetPath);
        Console.WriteLine("🎉 DONE! All transport lines successfully copied.");
    }

    static void UpdatePackageHeader(byte[] data, int delta)
    {
        using (MemoryStream ms = new MemoryStream(data))
        using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8))
        using (BinaryWriter bw = new BinaryWriter(ms, Encoding.UTF8))
        {
            br.ReadChars(4); // magic
            br.ReadUInt16(); // version
            br.ReadString(); // pkgName
            br.ReadString(); // author
            br.ReadUInt32(); // pkgVer
            br.ReadString(); // mainAsset
            int assetCount = br.ReadInt32();
            long dataOffset = br.ReadInt64();

            Console.WriteLine(string.Format("Updating Package Header: {0} assets, DataOffset={1}", assetCount, dataOffset));

            for (int i = 0; i < assetCount; i++)
            {
                br.ReadString(); // checksum
                string name = br.ReadString(); // name
                int type = br.ReadInt32(); // type

                long offsetPos = ms.Position;
                long offset = br.ReadInt64();

                long sizePos = ms.Position;
                long size = br.ReadInt64();

                if (i == 0)
                {
                    // Asset 0 is the simulation data containing TM -> its size changes by delta
                    long newSize = size + delta;
                    ms.Seek(sizePos, SeekOrigin.Begin);
                    bw.Write(newSize);
                    Console.WriteLine(string.Format("  Asset [0] '{0}': Size updated {1} -> {2}", name, size, newSize));
                    ms.Seek(sizePos + 8, SeekOrigin.Begin);
                }
                else
                {
                    // Assets 1..N come after simulation data -> their offsets change by delta
                    long newOffset = offset + delta;
                    ms.Seek(offsetPos, SeekOrigin.Begin);
                    bw.Write(newOffset);
                    Console.WriteLine(string.Format("  Asset [{0}] '{1}': Offset updated {2} -> {3}", i, name, offset, newOffset));
                    ms.Seek(sizePos + 8, SeekOrigin.Begin);
                }
            }
        }
    }

    static void VerifyPackage(byte[] data)
    {
        using (MemoryStream ms = new MemoryStream(data))
        using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8))
        {
            br.ReadChars(4);
            ushort version = br.ReadUInt16();
            string pkgName = br.ReadString();
            string author = br.ReadString();
            uint pkgVer = br.ReadUInt32();
            string mainAsset = br.ReadString();
            int assetCount = br.ReadInt32();
            long dataOffset = br.ReadInt64();

            Console.WriteLine(string.Format("Package: '{0}', Ver: {1}, Assets: {2}, DataOffset: {3}", pkgName, version, assetCount, dataOffset));
            for (int i = 0; i < assetCount; i++)
            {
                string checksum = br.ReadString();
                string name = br.ReadString();
                int type = br.ReadInt32();
                long offset = br.ReadInt64();
                long size = br.ReadInt64();
                Console.WriteLine(string.Format("  Asset [{0}] Name='{1}', Offset={2}, AbsOffset={3}, Size={4}",
                    i, name, offset, dataOffset + offset, size));
            }
        }
    }

    static int FindSignature(byte[] src, byte[] sig, int startIdx)
    {
        for (int i = startIdx; i <= src.Length - sig.Length; i++)
        {
            bool match = true;
            for (int j = 0; j < sig.Length; j++)
            {
                if (src[i + j] != sig[j]) { match = false; break; }
            }
            if (match) return i;
        }
        return -1;
    }
}
