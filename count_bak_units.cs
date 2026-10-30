using System;
using System.IO;
using System.Reflection;
using ColossalFramework.IO;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp.bak_original";
        using (var fs = File.OpenRead(path)) {
            // Find 00 2B at 58979323.
            // Where did PathManager start in bak_original?
            // Let's search backwards from 58979323 for the beginning of PathManager flags.
            // Or let's search for "PathManager" string in simulation data.
            // In bak_original, simulation data starts at 370.
            fs.Seek(370, SeekOrigin.Begin);
            var ds = (DataSerializer)typeof(DataSerializer).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Stream), typeof(DataSerializer.Mode), typeof(bool) }, null).Invoke(new object[] { fs, DataSerializer.Mode.File, false });
            typeof(DataSerializer).GetField("m_version", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ds, 121034u);

            // Let's check how many objects are in bak_original up to PathManager!
            // Or let's search for the EncodedArray.UShort start in bak_original.
            // In QuangNinh backup2.crp, PathManager started with fe 80 00 0b (unique string "PathManager").
            // Does bak_original have fe 80 00 0b or similar?
        }
    }
}
