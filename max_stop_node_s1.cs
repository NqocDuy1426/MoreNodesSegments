using System;
using System.IO;
using System.Reflection;
using ColossalFramework.IO;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup .crp";
        byte[] all = File.ReadAllBytes(path);
        byte[] pattern = new byte[] { 0xD4, 0x1E, 0xEF, 0x0D, 0x05, 0xFD, 0xFF, 0x03, 0x02, 0xD9, 0x14, 0x0D, 0xF3, 0x06, 0xFE, 0x0D };
        int tmOffset = -1;
        for (int i = all.Length - pattern.Length - 1; i >= 0; i--) {
            bool match = true;
            for (int j = 0; j < pattern.Length; j++) {
                if (all[i + j] != pattern[j]) { match = false; break; }
            }
            if (match && i > 10000) { tmOffset = i; break; }
        }

        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
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
            for (int i = 1; i < 256; i++) { if (flags[i] != 0) rPrefabs.Read(); }
            for (int i = 0; i < milestoneCount; i++) rPrefabs.Read();
            rPrefabs.EndRead();

            for (int i = 0; i < milestoneCount; i++) { ds.ReadInt32(); ds.ReadLong64(); }
            for (int i = 0; i < 17; i++) ds.ReadUInt16();

            int maxStop = 0;
            int count = 0;
            for (int i = 1; i < 256; i++) {
                if (flags[i] != 0) {
                    ds.ReadUInt16(); // lineNum
                    ushort stops = (ushort)ds.ReadUInt16();
                    ds.ReadUInt8(); ds.ReadUInt8(); ds.ReadUInt8(); ds.ReadUInt8(); // rgba
                    ds.ReadUInt16(); // bld
                    ds.ReadUInt16(); // budget
                    ds.ReadFloat(); // len
                    ds.ReadUInt8(); // avg
                    ds.ReadUInt16(); // ticket
                    if (stops > maxStop) maxStop = stops;
                    if (stops >= 65024) Console.WriteLine($"Line {i} has stop {stops} >= 65024!");
                    count++;
                }
            }
            Console.WriteLine($"Checked {count} lines. Max stop node = {maxStop}");
        }
    }
}
