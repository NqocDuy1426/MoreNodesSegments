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
                    // In DataSerializer header:
                    // Header tag: uint32
                    // Version: uint32
                    // Shared strings count: int32
                    // Strings: length (uint8 or uint16/32) + utf8 bytes
                    byte[] b4 = new byte[4];
                    stream.Read(b4, 0, 4); // header
                    stream.Read(b4, 0, 4); // version
                    uint version = (uint)(b4[0] << 24 | b4[1] << 16 | b4[2] << 8 | b4[3]);
                    Console.WriteLine("Version: " + version);

                    int stringCount = (stream.ReadByte() << 24) | (stream.ReadByte() << 16) | (stream.ReadByte() << 8) | stream.ReadByte();
                    Console.WriteLine("String count: " + stringCount);

                    var strings = new List<string>(stringCount);
                    for (int i = 0; i < stringCount; i++)
                    {
                        int len = stream.ReadByte();
                        if (len == 255)
                        {
                            len = (stream.ReadByte() << 8) | stream.ReadByte();
                        }
                        byte[] strBytes = new byte[len];
                        stream.Read(strBytes, 0, len);
                        string s = System.Text.Encoding.UTF8.GetString(strBytes);
                        strings.Add(s);
                        if (i == 1025 || s == "PathManager")
                        {
                            Console.WriteLine(string.Format("String[{0}] = '{1}'", i, s));
                        }
                    }

                    if (strings.Count > 1025)
                    {
                        Console.WriteLine("String[1025] is: '" + strings[1025] + "'");
                    }
                    else
                    {
                        Console.WriteLine("String count is ONLY: " + strings.Count + " (1025 is OUT OF RANGE!)");
                    }
                }
            }
        }
    }
}
