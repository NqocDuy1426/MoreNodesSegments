using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using ColossalFramework.IO;

public class CompareTransportData
{
    public static void Main()
    {
        string p1 = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup .crp";
        string p2 = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";

        int off1 = 45530463; // Offset of TransportManager in save 1
        int off2 = 45953494; // Offset of TransportManager in save 2

        Console.WriteLine("Reading Save 1 (QuangNinh backup)...");
        var data1 = ReadTM(p1, off1);

        Console.WriteLine("Reading Save 2 (QuangNinh backup2)...");
        var data2 = ReadTM(p2, off2);

        Console.WriteLine("\n--- Comparison ---");
        Console.WriteLine(string.Format("Save 1: {0} lines, Save 2: {1} lines", data1.Count, data2.Count));

        int matched = 0;
        int diffStops = 0;
        int diffFlags = 0;
        int onlyIn1 = 0;
        int onlyIn2 = 0;

        for (int i = 1; i < 256; i++)
        {
            bool has1 = data1.ContainsKey((ushort)i);
            bool has2 = data2.ContainsKey((ushort)i);

            if (has1 && !has2) onlyIn1++;
            else if (!has1 && has2) onlyIn2++;
            else if (has1 && has2)
            {
                var l1 = data1[(ushort)i];
                var l2 = data2[(ushort)i];
                if (l1.stops != l2.stops)
                {
                    diffStops++;
                    Console.WriteLine(string.Format("Line {0}: Stop count/node diff: Save1 stops={1}, Save2 stops={2}", i, l1.stops, l2.stops));
                }
                if (l1.flags != l2.flags)
                {
                    diffFlags++;
                    Console.WriteLine(string.Format("Line {0}: Flags diff: Save1=0x{1:X}, Save2=0x{2:X}", i, l1.flags, l2.flags));
                }
                matched++;
            }
        }

        Console.WriteLine(string.Format("Matched: {0}, DiffStops: {1}, DiffFlags: {2}, OnlyIn1: {3}, OnlyIn2: {4}",
            matched, diffStops, diffFlags, onlyIn1, onlyIn2));
    }

    public class LineInfo
    {
        public ushort id;
        public uint flags;
        public ushort lineNumber;
        public ushort stops;
        public ushort building;
    }

    static Dictionary<ushort, LineInfo> ReadTM(string path, int offset)
    {
        var result = new Dictionary<ushort, LineInfo>();
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            fs.Seek(offset + 16, SeekOrigin.Begin);
            ConstructorInfo ctor = typeof(DataSerializer).GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)[0];
            DataSerializer ds = (DataSerializer)ctor.Invoke(new object[] { fs, (DataSerializer.Mode)1, false });
            FieldInfo vFld = typeof(DataSerializer).GetField("m_version", BindingFlags.NonPublic | BindingFlags.Instance);
            if (vFld != null) vFld.SetValue(ds, 121034u);

            // 1. flags
            uint[] flags = new uint[256];
            EncodedArray.UInt rFlags = EncodedArray.UInt.BeginRead(ds);
            for (int i = 1; i < 256; i++) flags[i] = rFlags.Read();
            rFlags.EndRead();

            // 2. milestone count
            ushort milestoneCount = (ushort)ds.ReadUInt16();

            // 3. Prefabs
            // In standalone mono without PrefabCollection loaded, let's see:
            // PrefabCollection<TransportInfo>.BeginDeserialize(ds):
            // It reads a count and string array!
            // Let's see what PrefabCollection.BeginDeserialize reads:
            int prefabCount = (int)ds.ReadUInt16();
            for (int i = 0; i < prefabCount; i++)
            {
                ds.ReadSharedString();
            }

            // Then for each created line:
            // ReadUInt16() or ReadInt16() for prefab index!
            for (int i = 1; i < 256; i++)
            {
                if (flags[i] != 0)
                {
                    ds.ReadUInt16(); // prefab index
                }
            }

            // Milestone prefabs:
            for (int i = 0; i < milestoneCount; i++)
            {
                ds.ReadUInt16();
            }

            // Milestones data:
            for (int i = 0; i < milestoneCount; i++)
            {
                // MilestoneInfo.Data.Deserialize
                // Let's see what MilestoneInfo.Data reads:
                // typically ds.ReadObject
            }
        }
        return result;
    }
}
