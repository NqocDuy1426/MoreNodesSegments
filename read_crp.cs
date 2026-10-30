using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class CrpAsset
{
    public string Checksum;
    public string Name;
    public int Type;
    public long Offset;
    public long Size;
}

public class CrpReader
{
    public static void Main()
    {
        string savesDir = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves";
        string save1 = Path.Combine(savesDir, "QuangNinh backup .crp");
        string save2 = Path.Combine(savesDir, "QuangNinh backup2.crp");

        ReadCrp(save1);
        Console.WriteLine("\n--------------------------------------------------\n");
        ReadCrp(save2);
    }

    public static void ReadCrp(string path)
    {
        Console.WriteLine("Reading CRP: " + Path.GetFileName(path));
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        using (BinaryReader br = new BinaryReader(fs, Encoding.UTF8))
        {
            char[] magic = br.ReadChars(4);
            Console.WriteLine("Magic: " + new string(magic));
            ushort version = br.ReadUInt16();
            string pkgName = br.ReadString();
            string author = br.ReadString();
            uint pkgVer = br.ReadUInt32();
            string mainAsset = br.ReadString();
            int assetCount = br.ReadInt32();
            long dataOffset = br.ReadInt64();

            Console.WriteLine(string.Format("Pkg: '{0}', Ver: {1}, Main: '{2}', Assets: {3}, DataOffset: {4}",
                pkgName, version, mainAsset, assetCount, dataOffset));

            List<CrpAsset> assets = new List<CrpAsset>();
            for (int i = 0; i < assetCount; i++)
            {
                var a = new CrpAsset();
                a.Checksum = br.ReadString();
                a.Name = br.ReadString();
                a.Type = br.ReadInt32();
                a.Offset = br.ReadInt64();
                a.Size = br.ReadInt64();
                assets.Add(a);
                Console.WriteLine(string.Format("  [{0}] Name='{1}', Type={2}, RelOffset={3}, AbsOffset={4}, Size={5}",
                    i, a.Name, a.Type, a.Offset, dataOffset + a.Offset, a.Size));
            }
        }
    }
}
