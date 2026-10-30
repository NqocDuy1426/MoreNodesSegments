using System;
using System.IO;
using System.Collections.Generic;

class InspectFileTail
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        using (var fs = File.OpenRead(path))
        {
            fs.Seek(54370270, SeekOrigin.Begin);
            Console.WriteLine("Scanning from 54370270 to " + fs.Length + " (" + (fs.Length - fs.Position) + " bytes)...");
            
            // Search for tag 254 (new string or new object)
            int b;
            long pos;
            while ((pos = fs.Position) < fs.Length)
            {
                b = fs.ReadByte();
                if (b == 254) // 0xFE
                {
                    // Could be a unique string!
                    long candidatePos = pos;
                    int b0 = fs.ReadByte();
                    if (b0 == 255) continue;
                    int b1 = fs.ReadByte();
                    int b2 = fs.ReadByte();
                    int len = (b0 << 16) | (b1 << 8) | b2;
                    bool isAscii = (len & 0x800000) != 0;
                    len &= ~0x800000;
                    if (len > 3 && len < 200)
                    {
                        char[] c = new char[len];
                        int running = 128;
                        bool valid = true;
                        for (int i = 0; i < len; i++)
                        {
                            int diff = fs.ReadByte();
                            if (diff < 0) { valid = false; break; }
                            running = (running + diff) & 0xFF;
                            if (running < 32 || running > 126) { valid = false; break; }
                            c[i] = (char)running;
                        }
                        if (valid)
                        {
                            string s = new string(c);
                            Console.WriteLine(string.Format("Found string at pos {0}: '{1}'", candidatePos, s));
                        }
                    }
                    fs.Seek(candidatePos + 1, SeekOrigin.Begin);
                }
            }
            Console.WriteLine("Scan complete.");
        }
    }
}
