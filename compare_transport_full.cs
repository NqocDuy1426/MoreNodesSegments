using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using ColossalFramework.IO;

public class CompareTransportFull
{
    public static void Main()
    {
        string p1 = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup .crp";
        string p2 = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";

        int off1 = 45530463;
        int off2 = 45953494;

        Console.WriteLine("Reading Save 1 (QuangNinh backup)...");
        var lines1 = ReadLines(p1, off1);

        Console.WriteLine("Reading Save 2 (QuangNinh backup2)...");
        var lines2 = ReadLines(p2, off2);

        Console.WriteLine(string.Format("\nTotal lines: Save1={0}, Save2={1}", lines1.Count, lines2.Count));

        int diffStops = 0;
        int diffFlags = 0;
        int identical = 0;

        foreach (var kvp in lines1)
        {
            ushort id = kvp.Key;
            var l1 = kvp.Value;
            if (lines2.ContainsKey(id))
            {
                var l2 = lines2[id];
                bool same = true;
                if (l1.stops != l2.stops)
                {
                    Console.WriteLine(string.Format("Line {0} ({1}): STOPS DIFF! Save1={2}, Save2={3}", id, l1.lineNumber, l1.stops, l2.stops));
                    diffStops++;
                    same = false;
                }
                if (l1.flags != l2.flags)
                {
                    Console.WriteLine(string.Format("Line {0} ({1}): FLAGS DIFF! Save1=0x{2:X}, Save2=0x{3:X}", id, l1.lineNumber, l1.flags, l2.flags));
                    diffFlags++;
                    same = false;
                }
                if (same) identical++;
            }
            else
            {
                Console.WriteLine(string.Format("Line {0} ({1}) missing in Save2!", id, l1.lineNumber));
            }
        }

        Console.WriteLine(string.Format("\nSummary: Identical={0}, DiffStops={1}, DiffFlags={2}", identical, diffStops, diffFlags));
    }

    public class Line
    {
        public ushort id;
        public uint flags;
        public ushort infoIndex;
        public ushort lineNumber;
        public ushort stops;
        public byte r, g, b, a;
        public ushort building;
        public ushort budget;
        public float totalLength;
        public byte avgInterval;
        public ushort ticketPrice;
    }

    static Dictionary<ushort, Line> ReadLines(string path, int offset)
    {
        var dict = new Dictionary<ushort, Line>();
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
            EncodedArray.UShort rPrefabs = EncodedArray.UShort.BeginRead(ds);
            ushort[] infoIndices = new ushort[256];
            for (int i = 1; i < 256; i++)
            {
                if (flags[i] != 0)
                {
                    infoIndices[i] = rPrefabs.Read();
                }
            }
            for (int i = 0; i < milestoneCount; i++)
            {
                rPrefabs.Read();
            }
            rPrefabs.EndRead();

            // 4. Milestone data
            for (int i = 0; i < milestoneCount; i++)
            {
                ds.ReadInt32(); // passedCount
                ds.ReadLong64(); // progress
            }

            // 5. line numbers for 17 transport types
            for (int i = 0; i < 17; i++)
            {
                ds.ReadUInt16();
            }

            // 6. Each line
            for (int i = 1; i < 256; i++)
            {
                if (flags[i] != 0)
                {
                    Line l = new Line();
                    l.id = (ushort)i;
                    l.flags = flags[i];
                    l.infoIndex = infoIndices[i];
                    l.lineNumber = (ushort)ds.ReadUInt16();
                    l.stops = (ushort)ds.ReadUInt16();
                    l.r = (byte)ds.ReadUInt8();
                    l.g = (byte)ds.ReadUInt8();
                    l.b = (byte)ds.ReadUInt8();
                    l.a = (byte)ds.ReadUInt8();
                    l.building = (ushort)ds.ReadUInt16();
                    l.budget = (ushort)ds.ReadUInt16();
                    l.totalLength = ds.ReadFloat();
                    l.avgInterval = (byte)ds.ReadUInt8();
                    l.ticketPrice = (ushort)ds.ReadUInt16();

                    dict[l.id] = l;
                }
            }
        }
        return dict;
    }
}
