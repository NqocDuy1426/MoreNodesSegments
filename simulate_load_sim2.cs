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
                    
                    // Construct DataSerializer instance
                    var ctor = typeof(DataSerializer).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Stream), typeof(DataSerializer.Mode), typeof(bool) }, null);
                    var ds = (DataSerializer)ctor.Invoke(new object[] { stream, DataSerializer.Mode.File, false });
                    
                    var stringList = (List<string>)typeof(DataSerializer).GetField("m_stringList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(ds);
                    Console.WriteLine("StringList count: " + (stringList != null ? stringList.Count : 0));

                    var typeList = (List<Type>)typeof(DataSerializer).GetField("m_typeList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(ds);
                    Console.WriteLine("TypeList count: " + (typeList != null ? typeList.Count : 0));

                    if (stringList != null)
                    {
                        Console.WriteLine("StringList[0] = " + (stringList.Count > 0 ? stringList[0] : "empty"));
                        if (stringList.Count > 1025)
                        {
                            Console.WriteLine("StringList[1025] = " + stringList[1025]);
                        }
                        else
                        {
                            Console.WriteLine(string.Format("StringList only has {0} strings! 1025 is out of bounds!", stringList.Count));
                        }
                        int pmIdx = stringList.IndexOf("PathManager");
                        Console.WriteLine("Index of 'PathManager' in stringList: " + pmIdx);
                    }
                }
            }
        }
    }
}
