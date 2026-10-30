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
                    // Scan the stream for the string "PathManager" or "MilestoneManager" or "TransportManager"
                    byte[] data = new byte[stream.Length];
                    stream.Read(data, 0, data.Length);
                    Console.WriteLine("Data length: " + data.Length);

                    // Search for ASCII/UTF8 "PathManager"
                    byte[] target = System.Text.Encoding.UTF8.GetBytes("PathManager");
                    for (int i = 0; i <= data.Length - target.Length; i++)
                    {
                        bool match = true;
                        for (int j = 0; j < target.Length; j++)
                        {
                            if (data[i + j] != target[j])
                            {
                                match = false;
                                break;
                            }
                        }
                        if (match)
                        {
                            Console.WriteLine(string.Format("Found 'PathManager' at offset {0} (0x{0:X8})", i));
                        }
                    }

                    // Search for "Milestone"
                    byte[] mTarget = System.Text.Encoding.UTF8.GetBytes("Milestone");
                    for (int i = 0; i <= data.Length - mTarget.Length; i++)
                    {
                        bool match = true;
                        for (int j = 0; j < mTarget.Length; j++)
                        {
                            if (data[i + j] != mTarget[j])
                            {
                                match = false;
                                break;
                            }
                        }
                        if (match)
                        {
                            Console.WriteLine(string.Format("Found 'Milestone' at offset {0} (0x{0:X8})", i));
                        }
                    }
                }
            }
        }
    }
}
