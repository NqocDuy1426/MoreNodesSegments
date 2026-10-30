using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using ColossalFramework.IO;

class Program {
    static void Main() {
        string[] files = new string[] {
            "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp",
            "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp.bak_original"
        };
        foreach (var file in files) {
            Console.WriteLine("========================================");
            Console.WriteLine(Path.GetFileName(file));
            using (var fs = File.OpenRead(file)) {
                fs.Seek(370, SeekOrigin.Begin);
                try {
                    var smd = DataSerializer.Deserialize<SimulationMetaData>(fs, DataSerializer.Mode.File);
                    Console.WriteLine("SimulationMetaData read! fs.Position: " + fs.Position);
                    Console.WriteLine("CityName: " + smd.m_CityName);
                    if (smd.m_modOverride != null) {
                        foreach (var kvp in smd.m_modOverride) {
                            Console.WriteLine(string.Format("  Override: {0} = {1}", kvp.Key, kvp.Value));
                        }
                    }
                } catch (Exception ex) {
                    Console.WriteLine("Error reading meta: " + ex.Message);
                }
            }
        }
    }
}
