using System;
using System.IO;
using System.Reflection;
using ColossalFramework.IO;

public class CompareTmLines
{
    public static void Main()
    {
        string p1 = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup .crp";
        string p2 = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";

        var lines1 = ReadLines(p1, 45530463);
        var lines2 = ReadLines(p2, 45953494);

        Console.WriteLine(string.Format("Save 1 lines: {0}, Save 2 lines: {1}", lines1.Count, lines2.Count));

        int diffCount = 0;
        foreach (var kvp in lines1)
        {
            ushort id = kvp.Key;
            LineData d1 = kvp.Value;
            if (!lines2.ContainsKey(id))
            {
                Console.WriteLine(string.Format("Line [{0}] missing in Save 2!", id));
                diffCount++;
            }
            else
            {
                LineData d2 = lines2[id];
                if (d1.Flags != d2.Flags || d1.Stops != d2.Stops || d1.LineNumber != d2.LineNumber)
                {
                    Console.WriteLine(string.Format("Line [{0}] differs: S1(flags={1:X}, stops={2}, num={3}) vs S2(flags={4:X}, stops={5}, num={6})",
                        id, d1.Flags, d1.Stops, d1.LineNumber, d2.Flags, d2.Stops, d2.LineNumber));
                    diffCount++;
                }
            }
        }
        Console.WriteLine("Total line differences: " + diffCount);
    }

    class LineData
    {
        public uint Flags;
        public ushort Stops;
        public ushort LineNumber;
    }

    static System.Collections.Generic.Dictionary<ushort, LineData> ReadLines(string path, int offset)
    {
        var dict = new System.Collections.Generic.Dictionary<ushort, LineData>();
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            fs.Seek(offset + 16, SeekOrigin.Begin);

            ConstructorInfo ctor = typeof(DataSerializer).GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)[0];
            DataSerializer ds = (DataSerializer)ctor.Invoke(new object[] { fs, (DataSerializer.Mode)1, false });

            FieldInfo vFld = typeof(DataSerializer).GetField("m_version", BindingFlags.NonPublic | BindingFlags.Instance);
            if (vFld != null) vFld.SetValue(ds, 121034u);

            // 1. flags
            uint[] flags = new uint[256];
            EncodedArray.UInt reader = EncodedArray.UInt.BeginRead(ds);
            for (int i = 1; i < 256; i++) flags[i] = reader.Read();
            reader.EndRead();

            // 2. Prefab infoIndex (version >= 52)
            ushort[] infoIndex = new ushort[256];
            EncodedArray.UShort rInfo = EncodedArray.UShort.BeginRead(ds);
            for (int i = 1; i < 256; i++) infoIndex[i] = rInfo.Read();
            rInfo.EndRead();

            // 3. LineNumber (version >= 52)
            ushort[] lineNum = new ushort[256];
            EncodedArray.UShort rNum = EncodedArray.UShort.BeginRead(ds);
            for (int i = 1; i < 256; i++) lineNum[i] = rNum.Read();
            rNum.EndRead();

            // 4. Stops (version >= 52)
            ushort[] stops = new ushort[256];
            EncodedArray.UShort rStops = EncodedArray.UShort.BeginRead(ds);
            for (int i = 1; i < 256; i++) stops[i] = rStops.Read();
            rStops.EndRead();

            for (ushort i = 1; i < 256; i++)
            {
                if ((flags[i] & 1) != 0)
                {
                    dict[i] = new LineData { Flags = flags[i], Stops = stops[i], LineNumber = lineNum[i] };
                }
            }
        }
        return dict;
    }
}
