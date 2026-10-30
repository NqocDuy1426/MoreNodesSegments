using System;
using System.IO;
using ColossalFramework.IO;

class Program {
    static void Main() {
        string[] files = new string[] {
            "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp",
            "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp.bak_original"
        };
        long[] offsets = new long[] { 55201884, 59763291 };
        for (int i = 0; i < files.Length; i++) {
            Console.WriteLine("===============================");
            Console.WriteLine(Path.GetFileName(files[i]));
            using (var fs = File.OpenRead(files[i])) {
                fs.Seek(offsets[i], SeekOrigin.Begin);
                var smd = DataSerializer.Deserialize<SaveGameMetaData>(fs, DataSerializer.Mode.File);
                Console.WriteLine("CityName: " + smd.cityName);
                Console.WriteLine("TimeStamp: " + smd.timeStamp);
                Console.WriteLine("Mod count: " + (smd.mods != null ? smd.mods.Length : 0));
                if (smd.mods != null) {
                    foreach (var m in smd.mods) {
                        if (m.modName.Contains("Capacity") || m.modName.Contains("PathUnit") || m.modName.Contains("Vehicle"))
                            Console.WriteLine("  Mod: " + m.modName + " (id=" + m.modWorkshopID + ")");
                    }
                }
            }
        }
    }
}
