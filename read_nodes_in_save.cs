using System;
using System.IO;
using System.Collections;
using System.Reflection;
using ColossalFramework.IO;
using ColossalFramework.Packaging;

class ReadNodes
{
    static void Main()
    {
        string savePath = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/backup4.crp";
        using (var fs = File.OpenRead(savePath))
        {
            var pkg = new Package(null, savePath);
            var field = typeof(Package).GetField("m_IndexTable", BindingFlags.NonPublic | BindingFlags.Instance);
            var dict = (IDictionary)field.GetValue(pkg);

            foreach (DictionaryEntry entry in dict)
            {
                Package.Asset asset = (Package.Asset)entry.Value;
                if (asset.name != null && asset.name.EndsWith("_Data"))
                {
                    Console.WriteLine("Asset: " + asset.name + " offset: " + asset.offset + " size: " + asset.size);
                }
            }
        }
    }
}
