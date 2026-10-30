using System;
using System.IO;
using System.Reflection;
using ColossalFramework.IO;

public class ParseTransportExact
{
    public static void Main()
    {
        string p1 = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup .crp";
        string p2 = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";

        ParseSave(p1, 45530463, "Save 1 (QuangNinh backup)");
        ParseSave(p2, 45953494, "Save 2 (QuangNinh backup2)");
    }

    static void ParseSave(string path, int offset, string name)
    {
        Console.WriteLine("\n=== " + name + " ===");
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            fs.Seek(offset + 16, SeekOrigin.Begin);
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
            ushort[] infoIndices = new ushort[256];
            for (int i = 1; i < 256; i++)
            {
                if (flags[i] != 0) infoIndices[i] = rPrefabs.Read();
            }
            for (int i = 0; i < milestoneCount; i++) rPrefabs.Read();
            rPrefabs.EndRead();

            for (int i = 0; i < milestoneCount; i++)
            {
                ds.ReadInt32();
                ds.ReadLong64();
            }

            for (int i = 0; i < 17; i++) ds.ReadUInt16();

            int activeCount = 0;
            for (int i = 1; i < 256; i++)
            {
                if (flags[i] != 0)
                {
                    activeCount++;
                    ushort lineNum = (ushort)ds.ReadUInt16();
                    ushort stops = (ushort)ds.ReadUInt16();
                    byte r = (byte)ds.ReadUInt8();
                    byte g = (byte)ds.ReadUInt8();
                    byte b = (byte)ds.ReadUInt8();
                    ushort building = (ushort)ds.ReadUInt16();
                    ushort budget = (ushort)ds.ReadUInt16();
                    float length = ds.ReadFloat();
                    byte interval = (byte)ds.ReadUInt8();
                    ushort ticket = (ushort)ds.ReadUInt16();

                    // 8 groups * 3 UInt24
                    for (int gIdx = 0; gIdx < 8; gIdx++)
                    {
                        ds.ReadUInt24();
                        ds.ReadUInt24();
                        ds.ReadUInt24();
                    }

                    // ReadUniqueString
                    string vehicleSelection = ds.ReadUniqueString();

                    Console.WriteLine(string.Format("Line {0,3}: Num={1,3}, StopsNode={2,5}, Prefab={3,3}, Length={4,8:F1}, Vehicle={5}",
                        i, lineNum, stops, infoIndices[i], length, vehicleSelection));
                }
            }
            Console.WriteLine("Total active transport lines: " + activeCount);
        }
    }
}
