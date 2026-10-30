using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using ColossalFramework.IO;
using ColossalFramework.Packaging;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        using (var fs = File.OpenRead(path))
        {
            var pkg = Package.Deserialize(fs);
            Console.WriteLine("Package name: " + pkg.packageName);
            foreach (var asset in pkg)
            {
                Console.WriteLine("Asset: " + asset.name + " (" + asset.type + ") offset=" + asset.offset + " size=" + asset.size);
                if (asset.name == "SimulationMetaData")
                {
                    using (var mem = new MemoryStream(asset.GetBytes()))
                    {
                        var ctor = typeof(DataSerializer).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Stream), typeof(DataSerializer.Mode), typeof(bool) }, null);
                        var ds = (DataSerializer)ctor.Invoke(new object[] { mem, DataSerializer.Mode.File, false });
                        var smd = DataSerializer.Deserialize<SimulationMetaData>(mem, DataSerializer.Mode.File);
                        Console.WriteLine("  CityName: " + smd.m_CityName);
                        Console.WriteLine("  ModOverrides:");
                        if (smd.m_modOverride != null)
                        {
                            foreach (var kvp in smd.m_modOverride)
                            {
                                Console.WriteLine("    " + kvp.Key + " => " + kvp.Value);
                            }
                        }
                    }
                }
            }
        }
    }
}
