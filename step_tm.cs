using System;
using System.IO;
using System.Reflection;
using ColossalFramework.IO;

public class StepTm
{
    public static void Main()
    {
        Step("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup .crp", 45530463, "Save 1");
        Step("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp", 45953494, "Save 2");
    }

    static void Step(string path, int offset, string label)
    {
        Console.WriteLine("\n=== " + label + " ===");
        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            fs.Seek(offset + 16, SeekOrigin.Begin);
            ConstructorInfo ctor = typeof(DataSerializer).GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)[0];
            DataSerializer ds = (DataSerializer)ctor.Invoke(new object[] { fs, (DataSerializer.Mode)1, false });
            FieldInfo vFld = typeof(DataSerializer).GetField("m_version", BindingFlags.NonPublic | BindingFlags.Instance);
            if (vFld != null) vFld.SetValue(ds, 121034u);

            Console.WriteLine("Start pos: " + fs.Position);
            var rFlags = EncodedArray.UInt.BeginRead(ds);
            uint[] flags = new uint[256];
            int active = 0;
            for (int i = 1; i < 256; i++)
            {
                flags[i] = rFlags.Read();
                if (flags[i] != 0) active++;
            }
            rFlags.EndRead();
            Console.WriteLine(string.Format("Flags read. Active lines: {0}. Pos: {1}", active, fs.Position));

            ushort milestoneCount = (ushort)ds.ReadUInt16();
            Console.WriteLine(string.Format("MilestoneCount: {0}. Pos: {1}", milestoneCount, fs.Position));

            var rPrefabs = EncodedArray.UShort.BeginRead(ds);
            ushort[] linePrefabs = new ushort[256];
            for (int i = 1; i < 256; i++)
            {
                if (flags[i] != 0) linePrefabs[i] = rPrefabs.Read();
            }
            for (int i = 0; i < milestoneCount; i++)
            {
                rPrefabs.Read();
            }
            rPrefabs.EndRead();
            Console.WriteLine("Prefabs read. Pos: " + fs.Position);

            ushort prefabCount = (ushort)ds.ReadUInt16();
            Console.WriteLine("Prefab string count: " + prefabCount + ". Pos: " + fs.Position);
            for (int i = 0; i < prefabCount; i++)
            {
                string s = ds.ReadUniqueString();
                Console.WriteLine(string.Format("  [{0}] {1}", i, s));
            }
            Console.WriteLine("After prefab strings. Pos: " + fs.Position);

            // Read milestones
            Console.WriteLine("Reading " + milestoneCount + " milestone objects...");
            for (int i = 0; i < milestoneCount; i++)
            {
                // In ColossalFramework, ReadObject without custom resolver:
                int tag = fs.ReadByte();
                // Let's print tag
                if (i < 3) Console.WriteLine(string.Format("  Milestone [{0}] tag: {1} (0x{1:X2}) at {2}", i, tag, fs.Position - 1));
                if (tag == 254)
                {
                    // shared type
                    int subtag = fs.ReadByte();
                    // ...
                }
            }
        }
    }
}
