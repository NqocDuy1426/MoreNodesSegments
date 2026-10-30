using System;
using System.IO;
using System.Reflection;
using ColossalFramework.IO;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        using (var fs = File.OpenRead(path)) {
            fs.Seek(45760907, SeekOrigin.Begin);
            var ds = (DataSerializer)typeof(DataSerializer).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Stream), typeof(DataSerializer.Mode), typeof(bool) }, null).Invoke(new object[] { fs, DataSerializer.Mode.File, false });
            typeof(DataSerializer).GetField("m_version", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ds, 121034u);

            string tag = ds.ReadSharedString();
            var uShort = EncodedArray.UShort.BeginRead(ds);
            
            int total = 524288;
            ushort[] flags = new ushort[total];
            int active = 0;
            for (int i = 1; i < total; i++) {
                flags[i] = uShort.Read();
                if (flags[i] != 0) active++;
            }
            uShort.EndRead();
            
            int count = 0;
            for (int j = 1; j < total; j++) {
                if (flags[j] != 0) {
                    count++;
                    long uPos = fs.Position;
                    byte pff = (byte)ds.ReadUInt8();
                    byte lt = (byte)ds.ReadUInt8();
                    uint vt = ds.ReadUInt32();
                    byte pc = (byte)ds.ReadUInt8();
                    uint npu = ds.ReadUInt24();
                    uint bi = ds.ReadUInt32();
                    float len = ds.ReadFloat();
                    byte sp = (byte)ds.ReadUInt8();
                    for (int k = 0; k < pc; k++) {
                        ds.ReadUInt16();
                        ds.ReadUInt8();
                        ds.ReadUInt8();
                    }
                    if (count >= active - 5) {
                        Console.WriteLine(string.Format("Active #{0} (unit index {1}): offset={2}, pc={3}, npu={4}, bi={5}, len={6:F1}", 
                            count, j, uPos, pc, npu, bi, len));
                    }
                }
            }
            Console.WriteLine(string.Format("Finished reading {0} units. Stream pos={1}", count, fs.Position));
            Console.WriteLine("Remaining bytes until 54370769: " + (54370769 - fs.Position));
        }
    }
}
