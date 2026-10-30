using System;
using System.IO;
using System.Reflection;
using ColossalFramework.IO;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        using (var fs = File.OpenRead(path))
        {
            fs.Seek(45760907, SeekOrigin.Begin);
            var ctor = typeof(DataSerializer).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Stream), typeof(DataSerializer.Mode), typeof(bool) }, null);
            var ds = (DataSerializer)ctor.Invoke(new object[] { fs, DataSerializer.Mode.File, false });
            typeof(DataSerializer).GetField("m_version", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ds, 121034u);

            ds.ReadSharedString();
            var uShort = EncodedArray.UShort.BeginRead(ds);
            int length = 524288;
            ushort[] flags = new ushort[length];
            for (int i = 1; i < length; i++) flags[i] = uShort.Read();
            uShort.EndRead();

            int count = 0;
            int lastUnit = 0;
            for (int j = 1; j < length; j++)
            {
                if (flags[j] != 0)
                {
                    count++;
                    lastUnit = j;
                    long startPos = fs.Position;
                    byte pff = (byte)ds.ReadUInt8();
                    byte lt = (byte)ds.ReadUInt8();
                    uint vt = ds.ReadUInt32();
                    byte pc = (byte)ds.ReadUInt8();
                    uint npu = ds.ReadUInt24();
                    uint bi = ds.ReadUInt32();
                    float l = ds.ReadFloat();
                    byte sp = (byte)ds.ReadUInt8();
                    for (int k = 0; k < pc; k++)
                    {
                        ds.ReadUInt16();
                        ds.ReadUInt8();
                        ds.ReadUInt8();
                    }
                    long endPos = fs.Position;
                    if (count > 118190)
                    {
                        Console.WriteLine(string.Format("Active #{0} (Unit {1}): start={2}, end={3}, pc={4}, vt={5}, bi={6}, len={7}",
                            count, j, startPos, endPos, pc, vt, bi, l));
                    }
                }
            }
            Console.WriteLine(string.Format("Total active: {0}, last unit index: {1}, final pos: {2}", count, lastUnit, fs.Position));
        }
    }
}
