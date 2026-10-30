using System;
using System.IO;

class Program
{
    static void Main()
    {
        string p1 = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        string p2 = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp.bak_original";

        Console.WriteLine("QuangNinh backup2.crp size: " + new FileInfo(p1).Length);
        Console.WriteLine("bak_original size: " + new FileInfo(p2).Length);

        byte[] pat = DeltaEncode("PathManager");
        FindPat(p1, pat, "backup2");
        FindPat(p2, pat, "bak_original");
    }

    static void FindPat(string path, byte[] pat, string label)
    {
        byte[] all = File.ReadAllBytes(path);
        for (int i = 0; i <= all.Length - pat.Length; i++)
        {
            bool match = true;
            for (int j = 0; j < pat.Length; j++)
            {
                if (all[i + j] != pat[j]) { match = false; break; }
            }
            if (match)
            {
                Console.WriteLine(string.Format("{0}: Found 'PathManager' at offset {1} (0x{1:X8})", label, i));
            }
        }
    }

    static byte[] DeltaEncode(string s)
    {
        byte[] res = new byte[s.Length];
        int running = 128;
        for (int i = 0; i < s.Length; i++)
        {
            int target = (int)s[i];
            int diff = (target - running) & 0xFF;
            res[i] = (byte)diff;
            running = target;
        }
        return res;
    }
}
