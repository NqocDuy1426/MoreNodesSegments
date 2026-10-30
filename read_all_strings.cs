using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ColossalFramework.Packaging;

class Program
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
                    
                    // In DataSerializer:
                    // Read header
                    // Let's see how DataSerializer is deserialized in Assembly-CSharp
                }
            }
        }
    }
}
