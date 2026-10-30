using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using ColossalFramework.Packaging;

public class PatchSaveTransport
{
    public static void Main()
    {
        string savesDir = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves";
        string save1Path = Path.Combine(savesDir, "QuangNinh backup .crp");
        string save2Path = Path.Combine(savesDir, "QuangNinh backup2.crp");
        string patchedPath = Path.Combine(savesDir, "QuangNinh backup2.crp.patched");

        Console.WriteLine("=== Starting TransportManager Patch ===");
        Console.WriteLine("Source Clean TM (Save 1): " + save1Path);
        Console.WriteLine("Target Broken TM (Save 2): " + save2Path);

        int tm1Start = 45530463;
        int tm1End = 45572549;
        int tm1Len = tm1End - tm1Start; // 42086 bytes

        int tm2Start = 45953494;
        int tm2End = 45995702;
        int tm2Len = tm2End - tm2Start; // 42208 bytes

        int delta = tm1Len - tm2Len; // -122 bytes
        Console.WriteLine(string.Format("TM1 size: {0}, TM2 size: {1}, Delta: {2}", tm1Len, tm2Len, delta));

        // Read TM chunk from Save 1
        byte[] tmChunk = new byte[tm1Len];
        using (FileStream fs1 = new FileStream(save1Path, FileMode.Open, FileAccess.Read))
        {
            fs1.Seek(tm1Start, SeekOrigin.Begin);
            int read = fs1.Read(tmChunk, 0, tm1Len);
            if (read != tm1Len) throw new Exception("Failed to read TM chunk from Save 1");
        }

        // Verify TM chunk signature: "TransportManager" pattern
        byte[] tmSig = new byte[] { 0xD4, 0x1E, 0xEF, 0x0D, 0x05, 0xFD, 0xFF, 0x03, 0x02, 0xD9, 0x14, 0x0D, 0xF3, 0x06, 0xFE, 0x0D };
        for (int i = 0; i < tmSig.Length; i++)
        {
            if (tmChunk[i] != tmSig[i]) throw new Exception("TM signature mismatch at byte " + i);
        }
        Console.WriteLine("Verified Clean TM chunk signature.");

        // Read Save 2 completely
        byte[] s2 = File.ReadAllBytes(save2Path);
        Console.WriteLine(string.Format("Save 2 original length: {0} bytes", s2.Length));

        // Verify Save 2 TM start and end
        for (int i = 0; i < tmSig.Length; i++)
        {
            if (s2[tm2Start + i] != tmSig[i]) throw new Exception("Save 2 TM signature mismatch at byte " + i);
        }
        byte[] dmSig = new byte[] { 0xC4, 0x25, 0x0A, 0x01, 0xFE, 0xF7, 0xFA, 0x11, 0xD9, 0x14, 0x0D, 0xF3, 0x06, 0xFE, 0x0D };
        for (int i = 0; i < dmSig.Length; i++)
        {
            if (s2[tm2End + i] != dmSig[i]) throw new Exception("Save 2 DM signature mismatch at byte " + i);
        }
        Console.WriteLine("Verified Save 2 TM chunk bounds and DistrictManager start.");

        // Create new patched buffer
        int newLength = s2.Length + delta;
        byte[] patched = new byte[newLength];

        // 1. Copy everything before TM2: [0 .. tm2Start)
        Buffer.BlockCopy(s2, 0, patched, 0, tm2Start);

        // 2. Copy clean TM1 chunk: [tm2Start .. tm2Start + tm1Len)
        Buffer.BlockCopy(tmChunk, 0, patched, tm2Start, tm1Len);

        // 3. Copy everything after TM2: [tm2End .. s2.Length) to [tm2Start + tm1Len .. newLength)
        int remainderLen = s2.Length - tm2End;
        Buffer.BlockCopy(s2, tm2End, patched, tm2Start + tm1Len, remainderLen);

        // 4. Update Header in patched:
        // Asset 0 Size at byte 127
        long oldAsset0Size = BitConverter.ToInt64(patched, 127);
        long newAsset0Size = oldAsset0Size + delta;
        byte[] bAsset0Size = BitConverter.GetBytes(newAsset0Size);
        Buffer.BlockCopy(bAsset0Size, 0, patched, 127, 8);

        // Asset 1 Offset at byte 203
        long oldAsset1Offset = BitConverter.ToInt64(patched, 203);
        long newAsset1Offset = oldAsset1Offset + delta;
        byte[] bAsset1Offset = BitConverter.GetBytes(newAsset1Offset);
        Buffer.BlockCopy(bAsset1Offset, 0, patched, 203, 8);

        // Asset 2 Offset at byte 283
        long oldAsset2Offset = BitConverter.ToInt64(patched, 283);
        long newAsset2Offset = oldAsset2Offset + delta;
        byte[] bAsset2Offset = BitConverter.GetBytes(newAsset2Offset);
        Buffer.BlockCopy(bAsset2Offset, 0, patched, 283, 8);

        // Asset 3 Offset at byte 354
        long oldAsset3Offset = BitConverter.ToInt64(patched, 354);
        long newAsset3Offset = oldAsset3Offset + delta;
        byte[] bAsset3Offset = BitConverter.GetBytes(newAsset3Offset);
        Buffer.BlockCopy(bAsset3Offset, 0, patched, 354, 8);

        Console.WriteLine(string.Format("Updated Header: Asset 0 Size = {0} -> {1}", oldAsset0Size, newAsset0Size));
        Console.WriteLine(string.Format("Updated Header: Asset 1 Offset = {0} -> {1}", oldAsset1Offset, newAsset1Offset));
        Console.WriteLine(string.Format("Updated Header: Asset 2 Offset = {0} -> {1}", oldAsset2Offset, newAsset2Offset));
        Console.WriteLine(string.Format("Updated Header: Asset 3 Offset = {0} -> {1}", oldAsset3Offset, newAsset3Offset));

        // Write to patchedPath
        File.WriteAllBytes(patchedPath, patched);
        Console.WriteLine("Wrote patched file to: " + patchedPath);

        // Verification:
        Console.WriteLine("\n=== Verifying Patched Package ===");
        VerifyPackage(patchedPath);

        // Manager verification:
        Console.WriteLine("\n=== Verifying Manager Offsets in Patched Package ===");
        VerifyManagers(patchedPath);

        // Safe replace:
        string backupFile = save2Path + ".bak_before_tm_replace";
        if (!File.Exists(backupFile))
        {
            File.Copy(save2Path, backupFile);
            Console.WriteLine("Created backup: " + backupFile);
        }

        File.Copy(patchedPath, save2Path, true);
        Console.WriteLine("\nSUCCESSFULLY REPLACED: " + save2Path);
    }

    static void VerifyPackage(string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        using (BinaryReader br = new BinaryReader(fs, Encoding.UTF8))
        {
            char[] magic = br.ReadChars(4);
            ushort version = br.ReadUInt16();
            string pkgName = br.ReadString();
            string author = br.ReadString();
            uint pkgVer = br.ReadUInt32();
            string mainAsset = br.ReadString();
            int assetCount = br.ReadInt32();
            long dataOffset = br.ReadInt64();

            Console.WriteLine(string.Format("Pkg: '{0}', Ver: {1}, Main: '{2}', Assets: {3}, DataOffset: {4}",
                pkgName, version, mainAsset, assetCount, dataOffset));

            for (int i = 0; i < assetCount; i++)
            {
                string checksum = br.ReadString();
                string name = br.ReadString();
                int type = br.ReadInt32();
                long offset = br.ReadInt64();
                long size = br.ReadInt64();
                Console.WriteLine(string.Format("  [{0}] Name='{1}', Type={2}, RelOffset={3}, AbsOffset={4}, Size={5}",
                    i, name, type, offset, dataOffset + offset, size));
            }
        }
    }

    static void VerifyManagers(string path)
    {
        string[] mgrs = new string[] {
            "VehicleManager", "TransportManager", "DistrictManager", "DisasterManager"
        };
        byte[] all = File.ReadAllBytes(path);

        foreach (var m in mgrs)
        {
            byte[] pat = EncodeString(m);
            int found = -1;
            for (int i = 10000; i <= all.Length - pat.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < pat.Length; j++)
                {
                    if (all[i + j] != pat[j]) { match = false; break; }
                }
                if (match) { found = i; break; }
            }
            Console.WriteLine(string.Format("  Manager {0,-18} Offset: {1}", m, found));
        }
    }

    static byte[] EncodeString(string str)
    {
        List<byte> bytes = new List<byte>();
        int running = 128;
        foreach (char c in str)
        {
            int diff = ((int)c - running) & 0xFF;
            bytes.Add((byte)diff);
            running = (int)c;
        }
        return bytes.ToArray();
    }
}
