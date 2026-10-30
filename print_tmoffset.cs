using System;
using System.IO;

class Program
{
    static void Main()
    {
        Find("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup .crp");
        Find("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp");
    }

    static void Find(string path)
    {
        byte[] all = File.ReadAllBytes(path);
        byte[] pattern = new byte[] { 0xD4, 0x1E, 0xEF, 0x0D, 0x05, 0xFD, 0xFF, 0x03, 0x02, 0xD9, 0x14, 0x0D, 0xF3, 0x06, 0xFE, 0x0D };
        for (int i = 0; i <= all.Length - pattern.Length; i++)
        {
            bool match = true;
            for (int j = 0; j < pattern.Length; j++)
            {
                if (all[i + j] != pattern[j]) { match = false; break; }
            }
            if (match)
            {
                Console.WriteLine(Path.GetFileName(path) + " found pattern at " + i);
            }
        }
    }
}
