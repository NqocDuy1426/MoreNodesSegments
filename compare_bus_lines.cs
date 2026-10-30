using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using ColossalFramework.IO;

public class CompareBusLines
{
    public static void Main()
    {
        string savesDir = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves";
        string s1 = Path.Combine(savesDir, "QuangNinh backup .crp");
        string s2 = Path.Combine(savesDir, "QuangNinh backup2.crp");

        Console.WriteLine("=== S1: QuangNinh backup .crp ===");
        var lines1 = ParseLines(s1);
        Console.WriteLine(string.Format("S1 Valid Lines: {0}", lines1.Count));

        Console.WriteLine("\n=== S2: QuangNinh backup2.crp ===");
        var lines2 = ParseLines(s2);
        Console.WriteLine(string.Format("S2 Valid Lines: {0}", lines2.Count));

        int common = 0;
        int diff = 0;
        foreach (var kvp in lines1)
        {
            if (lines2.ContainsKey(kvp.Key))
            {
                var l1 = kvp.Value;
                var l2 = lines2[kvp.Key];
                if (l1.Stops == l2.Stops && l1.LineNumber == l2.LineNumber)
                {
                    common++;
                }
                else
                {
                    diff++;
                    if (diff <= 10)
                        Console.WriteLine(string.Format("Diff Line {0}: S1(stops={1}, num={2}, len={3}) vs S2(stops={4}, num={5}, len={6})",
                            kvp.Key, l1.Stops, l1.LineNumber, l1.TotalLength, l2.Stops, l2.LineNumber, l2.TotalLength));
                }
            }
        }
        Console.WriteLine(string.Format("\nSummary: Common identical: {0}, Differing: {1}", common, diff));
    }

    public class LineInfo
    {
        public ushort Id;
        public uint Flags;
        public ushort InfoIndex;
        public ushort LineNumber;
        public ushort Stops;
        public float TotalLength;
        public byte R, G, B;
    }

    static Dictionary<ushort, LineInfo> ParseLines(string path)
    {
        var dict = new Dictionary<ushort, LineInfo>();
        byte[] all = File.ReadAllBytes(path);
        byte[] pattern = new byte[] { 0xD4, 0x1E, 0xEF, 0x0D, 0x05, 0xFD, 0xFF, 0x03, 0x02, 0xD9, 0x14, 0x0D, 0xF3, 0x06, 0xFE, 0x0D };
        int tmOffset = -1;
        for (int i = all.Length - pattern.Length - 1; i >= 0; i--)
        {
            bool match = true;
            for (int j = 0; j < pattern.Length; j++)
            {
                if (all[i + j] != pattern[j]) { match = false; break; }
            }
            if (match && i > 10000) { tmOffset = i; break; }
        }
        if (tmOffset < 0) { Console.WriteLine("TM chunk not found in " + path); return dict; }

        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            fs.Seek(tmOffset + 16, SeekOrigin.Begin);
            ConstructorInfo ctor = typeof(DataSerializer).GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)[0];
            DataSerializer ds = (DataSerializer)ctor.Invoke(new object[] { fs, (DataSerializer.Mode)1, false });
            FieldInfo vFld = typeof(DataSerializer).GetField("m_version", BindingFlags.NonPublic | BindingFlags.Instance);
            if (vFld != null) vFld.SetValue(ds, 121034u);

            uint[] flags = new uint[256];
            EncodedArray.UInt rFlags = EncodedArray.UInt.BeginRead(ds);
            for (int i = 1; i < 256; i++) flags[i] = rFlags.Read();
            rFlags.EndRead();

            ushort milestoneCount = (ushort)ds.ReadUInt16();
            EncodedArray.UShort rPrefabs = EncodedArray.UShort.BeginRead(ds);
            ushort[] prefabs = new ushort[256];
            for (int i = 1; i < 256; i++) { if (flags[i] != 0) prefabs[i] = rPrefabs.Read(); }
            for (int i = 0; i < milestoneCount; i++) rPrefabs.Read();
            rPrefabs.EndRead();

            for (int i = 0; i < milestoneCount; i++) { ds.ReadInt32(); ds.ReadLong64(); }
            for (int i = 0; i < 17; i++) ds.ReadUInt16();

            for (ushort i = 1; i < 256; i++)
            {
                if (flags[i] != 0)
                {
                    ushort lineNum = (ushort)ds.ReadUInt16();
                    ushort stops = (ushort)ds.ReadUInt16();
                    byte r = (byte)ds.ReadUInt8();
                    byte g = (byte)ds.ReadUInt8();
                    byte b = (byte)ds.ReadUInt8();
                    byte a = (byte)ds.ReadUInt8();
                    ushort bld = (ushort)ds.ReadUInt16();
                    ushort budget = (ushort)ds.ReadUInt16();
                    float len = ds.ReadFloat();
                    byte avg = (byte)ds.ReadUInt8();
                    ushort ticket = (ushort)ds.ReadUInt16();

                    if ((flags[i] & 1) != 0) // Created
                    {
                        dict[i] = new LineInfo {
                            Id = i, Flags = flags[i], InfoIndex = prefabs[i], LineNumber = lineNum,
                            Stops = stops, TotalLength = len, R = r, G = g, B = b
                        };
                    }
                }
            }
        }
        return dict;
    }
}
