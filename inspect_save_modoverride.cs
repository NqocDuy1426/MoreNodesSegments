using System;
using System.IO;
using System.Collections.Generic;
using ColossalFramework.Packaging;
using ColossalFramework.IO;

class Program
{
    static void Main()
    {
        string dir = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Application Support/Colossal Order/Cities_Skylines/Saves";
        foreach (var file in Directory.GetFiles(dir, "*.crp"))
        {
            try
            {
                var pkg = new Package(Path.GetFileNameWithoutExtension(file), file);
                Package.Asset metaAsset = null;
                foreach (Package.Asset a in pkg)
                {
                    if (a.name != null && a.name.EndsWith("_SimulationMetaData"))
                    {
                        metaAsset = a;
                        break;
                    }
                }
                if (metaAsset != null)
                {
                    using (var stream = metaAsset.GetStream())
                    {
                        var meta = DataSerializer.Deserialize<SimulationMetaData>(stream, DataSerializer.Mode.Memory);
                        Console.WriteLine("Save: " + Path.GetFileName(file));
                        if (meta != null && meta.m_modOverride != null)
                        {
                            Console.WriteLine("  modOverride count: " + meta.m_modOverride.Count);
                            foreach (var kvp in meta.m_modOverride)
                            {
                                Console.WriteLine("    [" + kvp.Key + "] = " + kvp.Value);
                            }
                        }
                        else
                        {
                            Console.WriteLine("  modOverride is null or empty!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // ignore
            }
        }
    }
}
