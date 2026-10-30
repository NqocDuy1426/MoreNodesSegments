using System;
using System.IO;
using System.Collections.Generic;

class ExtractAllSharedStrings
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        using (var fs = File.OpenRead(path))
        {
            // Skip CRP header
            fs.Seek(370, SeekOrigin.Begin);
            
            // Read version
            int b0 = fs.ReadByte(); int b1 = fs.ReadByte();
            int b2 = fs.ReadByte(); int b3 = fs.ReadByte();
            
            // In SimulationMetaData:
            // Let's trace shared strings
        }
        
        // Better: search the entire file for tag 254 strings
        byte[] all = File.ReadAllBytes(path);
        Console.WriteLine("Total size: " + all.Length);
        
        // Let's find every occurrence of 254 (0xFE) followed by unique string
        // But in DataSerializer, WriteSharedString writes:
        // stream.WriteByte(254);
        // WriteUniqueString(str);
        // And UniqueString has:
        // byte 0: length >> 16 (often 0x80 for ASCII)
        // byte 1: length >> 8
        // byte 2: length & 0xFF
        List<string> strings = new List<string>();
        List<long> offsets = new List<long>();
        
        for (int i = 370; i < all.Length - 4; i++)
        {
            if (all[i] == 254)
            {
                int lenB0 = all[i+1];
                if ((lenB0 & 0x80) != 0 && (lenB0 & 0x7F) == 0) // ASCII single byte length < 65536
                {
                    int len = ((lenB0 & 0x7F) << 16) | (all[i+2] << 8) | all[i+3];
                    if (len >= 3 && len <= 120 && i + 4 + len <= all.Length)
                    {
                        char[] c = new char[len];
                        int running = 128;
                        bool ok = true;
                        for (int k = 0; k < len; k++)
                        {
                            running = (running + all[i + 4 + k]) & 0xFF;
                            if (running < 32 || running > 126) { ok = false; break; }
                            c[k] = (char)running;
                        }
                        if (ok)
                        {
                            string s = new string(c);
                            strings.Add(s);
                            offsets.Add(i);
                        }
                    }
                }
            }
        }
        
        Console.WriteLine("Total unique strings found: " + strings.Count);
        for (int i = 0; i < Math.Min(30, strings.Count); i++)
        {
            Console.WriteLine(string.Format("  [{0}] @{1}: '{2}'", i, offsets[i], strings[i]));
        }
        Console.WriteLine("...");
        for (int i = Math.Max(0, strings.Count - 20); i < strings.Count; i++)
        {
            Console.WriteLine(string.Format("  [{0}] @{1}: '{2}'", i, offsets[i], strings[i]));
        }
    }
}
