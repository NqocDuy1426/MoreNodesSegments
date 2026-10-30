using System;
using System.IO;
using System.Collections;
using System.Reflection;
using ColossalFramework.Packaging;

class Program
{
    static void Main()
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
                Console.WriteLine("Extracting asset: " + asset.name + " (" + asset.size + " bytes)");
                using (var inStream = asset.GetStream())
                using (var outStream = File.Create("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/backup4_sim.bin"))
                {
                    inStream.CopyTo(outStream);
                    Console.WriteLine("Done! Extracted " + outStream.Length + " bytes.");
                }
                break;
            }
        }
    }
}
