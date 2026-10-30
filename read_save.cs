using System;
using System.IO;
using System.Collections;
using System.Reflection;
using ColossalFramework.Packaging;
using ColossalFramework.IO;

class ReadSave
{
    static void Main(string[] args)
    {
        string path = args.Length > 0 ? args[0] : "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        Console.WriteLine("Reading CRP: " + path);
        var pkg = new Package(null, path);
        var field = typeof(Package).GetField("m_IndexTable", BindingFlags.NonPublic | BindingFlags.Instance);
        var dict = (IDictionary)field.GetValue(pkg);
        foreach (DictionaryEntry entry in dict)
        {
            Package.Asset asset = (Package.Asset)entry.Value;
            if (asset.name != null && asset.name.EndsWith("_Data"))
            {
                using (var s = asset.GetStream())
                {
                    long startPos = s.Position;
                    Console.WriteLine("Asset offset in file: " + asset.offset + ", stream pos: " + startPos);
                    var meta = DataSerializer.Deserialize<SimulationMetaData>(s, DataSerializer.Mode.File);
                    Console.WriteLine("Meta OK. Position=" + s.Position + " (read " + (s.Position - startPos) + " bytes)");
                    if (meta.m_modOverride != null)
                    {
                        Console.WriteLine("m_modOverride count: " + meta.m_modOverride.Count);
                        foreach (var kvp in meta.m_modOverride)
                        {
                            Console.WriteLine("  Key: '" + kvp.Key + "' => " + kvp.Value);
                        }
                    }
                    else
                    {
                        Console.WriteLine("m_modOverride is NULL!");
                    }
                }
            }
        }
    }
}
