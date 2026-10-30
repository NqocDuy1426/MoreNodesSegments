using System;
using System.IO;
using System.Collections;
using System.Reflection;
using ColossalFramework;
using ColossalFramework.IO;
using ColossalFramework.Packaging;

class Program
{
    static void Main()
    {
        string gameManaged = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        AppDomain.CurrentDomain.AssemblyResolve += (sender, a) =>
        {
            string n = new AssemblyName(a.Name).Name;
            string p = Path.Combine(gameManaged, n + ".dll");
            if (File.Exists(p)) return Assembly.LoadFrom(p);
            p = Path.Combine("/Users/nqocduy/CS_MoreNetworkCapacity_Renew", n + ".dll");
            if (File.Exists(p)) return Assembly.LoadFrom(p);
            return null;
        };

        Run();
    }

    static void Run()
    {
        string savePath = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/backup4.crp";
        var pkg = new Package(null, savePath);
        var field = typeof(Package).GetField("m_IndexTable", BindingFlags.NonPublic | BindingFlags.Instance);
        var dict = (IDictionary)field.GetValue(pkg);

        foreach (DictionaryEntry entry in dict)
        {
            Package.Asset asset = (Package.Asset)entry.Value;
            if (asset.name != null && asset.name.EndsWith("_Data"))
            {
                Console.WriteLine("Found asset: " + asset.name + " (" + asset.size + " bytes)");
                using (var s = asset.GetStream())
                {
                    var meta = DataSerializer.Deserialize<SimulationMetaData>(s, DataSerializer.Mode.Memory);
                    Console.WriteLine("SimulationMetaData: " + meta.m_CityName + ", mode: " + meta.m_updateMode);
                    Console.WriteLine("Stream pos after meta: " + s.Position + " / " + s.Length);

                    // Now let's see how DataSerializer.Deserialize<SimulationManager.Data> reads managers
                    // Create a DataSerializer
                    var dsType = typeof(DataSerializer);
                    var desMethod = dsType.GetMethod("Deserialize", new Type[] { typeof(Stream), typeof(DataSerializer.Mode) });
                    var genMethod = desMethod.MakeGenericMethod(typeof(SimulationManager).GetNestedType("Data", BindingFlags.Public | BindingFlags.NonPublic));
                    
                    Console.WriteLine("Deserializing SimulationManager.Data...");
                    try
                    {
                        genMethod.Invoke(null, new object[] { s, DataSerializer.Mode.Memory });
                        Console.WriteLine("SimulationManager.Data deserialized successfully!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception in SimulationManager.Data: " + ex);
                    }
                }
            }
        }
    }
}
