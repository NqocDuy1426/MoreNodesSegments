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
                    Console.WriteLine("Asset length: " + stream.Length);
                    // Read from 54370260 onwards
                    stream.Seek(54370260, SeekOrigin.Begin);
                    byte[] buf = new byte[200];
                    stream.Read(buf, 0, buf.Length);
                    for (int i = 0; i < buf.Length; i++)
                    {
                        if (i % 16 == 0) Console.Write(string.Format("\n{0:X8}: ", 54370260 + i));
                        Console.Write(string.Format("{0:X2} ", buf[i]));
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
