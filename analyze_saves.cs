using System;
using System.IO;
using System.Collections;
using System.Reflection;
using ColossalFramework;
using ColossalFramework.IO;
using ColossalFramework.Packaging;

class Program
{
    static void Main(string[] args)
    {
        string gameManaged = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        AppDomain.CurrentDomain.AssemblyResolve += (sender, a) =>
        {
            string n = new AssemblyName(a.Name).Name;
            string p = Path.Combine(gameManaged, n + ".dll");
            if (File.Exists(p)) return Assembly.LoadFrom(p);
            return null;
        };

        AnalyzeSave("QuangNinh backup2.crp");
        AnalyzeSave("QuangNinh backup3.crp");
    }

    static void AnalyzeSave(string saveFileName)
    {
        string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + 
            "/Library/Application Support/Colossal Order/Cities_Skylines/Saves", saveFileName);
        long sz = new FileInfo(savePath).Length / 1024 / 1024;
        Console.WriteLine("\n=======================================================");
        Console.WriteLine("ANALYZING SAVE: " + saveFileName + " (Size: " + sz + " MB)");
        Console.WriteLine("=======================================================");

        var pkg = new Package(null, savePath);
        var field = typeof(Package).GetField("m_IndexTable", BindingFlags.NonPublic | BindingFlags.Instance);
        var dict = (IDictionary)field.GetValue(pkg);

        foreach (DictionaryEntry entry in dict)
        {
            Package.Asset asset = (Package.Asset)entry.Value;
            if (asset.name != null && asset.name.EndsWith("_Data"))
            {
                using (var s = asset.GetStream())
                {
                    var meta = DataSerializer.Deserialize<SimulationMetaData>(s, DataSerializer.Mode.File);
                    Console.WriteLine("City Name: " + meta.m_CityName);
                    Console.WriteLine("Environment: " + meta.m_environment);
                    if (meta.m_modOverride != null)
                    {
                        Console.WriteLine("Mod Overrides Count: " + meta.m_modOverride.Count);
                        foreach (var kv in meta.m_modOverride)
                        {
                            Console.WriteLine("  Override: " + kv.Key + " = " + kv.Value);
                        }
                    }
                }
            }
        }
    }
}
