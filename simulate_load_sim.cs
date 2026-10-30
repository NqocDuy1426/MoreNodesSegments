using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ColossalFramework.Packaging;
using ColossalFramework.IO;

class SimulateLoadSim
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        var pkg = new Package(null, path);
        var field = typeof(Package).GetField("m_IndexTable", BindingFlags.NonPublic | BindingFlags.Instance);
        var dict = (IDictionary)field.GetValue(pkg);
        foreach (DictionaryEntry entry in dict)
        {
            Package.Asset asset = (Package.Asset)entry.Value;
            if (asset.name != null && asset.name.EndsWith("_Data"))
            {
                using (var stream = asset.GetStream())
                {
                    Console.WriteLine("Asset found: " + asset.name + ", length=" + stream.Length);
                    
                    var meta = DataSerializer.Deserialize<SimulationMetaData>(stream, DataSerializer.Mode.File);
                    Console.WriteLine("1. SimulationMetaData read OK. Pos=" + stream.Position);

                    // Create DataSerializer on stream
                    var ctor = typeof(DataSerializer).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Stream), typeof(DataSerializer.Mode), typeof(bool) }, null);
                    var ds = (DataSerializer)ctor.Invoke(new object[] { stream, DataSerializer.Mode.File, false });
                    
                    // Inspect shared strings
                    var strTable = (List<string>)typeof(DataSerializer).GetField("s_sharedStringTable", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
                    Console.WriteLine("Shared string table count: " + (strTable != null ? strTable.Count : 0));

                    var typeTable = (List<Type>)typeof(DataSerializer).GetField("s_sharedTypeTable", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
                    Console.WriteLine("Shared type table count: " + (typeTable != null ? typeTable.Count : 0));
                    if (typeTable != null)
                    {
                        for (int i = 0; i < Math.Min(30, typeTable.Count); i++)
                        {
                            Console.WriteLine(string.Format("  type[{0}] = {1}", i, typeTable[i] != null ? typeTable[i].FullName : "null"));
                        }
                    }

                    // Let's read the tags from stream to see all managers!
                }
            }
        }
    }
}
