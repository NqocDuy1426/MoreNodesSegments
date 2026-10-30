using System;
using System.IO;
using System.Reflection;
using ColossalFramework.IO;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp.bak_original";
        using (var fs = File.OpenRead(path))
        {
            fs.Seek(46222037, SeekOrigin.Begin);
            var ctor = typeof(DataSerializer).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Stream), typeof(DataSerializer.Mode), typeof(bool) }, null);
            var ds = (DataSerializer)ctor.Invoke(new object[] { fs, DataSerializer.Mode.File, false });
            typeof(DataSerializer).GetField("m_version", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ds, 121034u);

            ds.ReadSharedString();
            var uShort = EncodedArray.UShort.BeginRead(ds);
            int length = 524288;
            ushort[] flags = new ushort[length];
            for (int i = 1; i < length; i++) flags[i] = uShort.Read();
            uShort.EndRead();

            for (int j = 1; j < length; j++)
            {
                if (flags[j] != 0)
                {
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
                    if (startPos <= 58978735 && endPos >= 58978735)
                    {
                        Console.WriteLine(string.Format("PathUnit {0}: startPos={1}, endPos={2}, pc={3}, vt={4}, next={5}, bi={6}, len={7}",
                            j, startPos, endPos, pc, vt, npu, bi, l));
                    }
                }
            }
        }
    }
}
