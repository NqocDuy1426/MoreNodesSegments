using System;
using System.IO;

class Program
{
    static void Main()
    {
        byte[] s1 = File.ReadAllBytes("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup .crp");
        byte[] s2 = File.ReadAllBytes("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp");

        int off1 = 45530463;
        int len1 = 45572549 - 45530463; // 42086

        int off2 = 45953494;
        int len2 = 42086;

        Console.WriteLine(string.Format("Comparing S1[{0}..{1}] ({2} bytes) with S2[{3}..{4}] ({5} bytes)",
            off1, off1 + len1, len1, off2, off2 + len2, len2));

        int diffs = 0;
        for (int i = 0; i < len1; i++)
        {
            if (s1[off1 + i] != s2[off2 + i])
            {
                if (diffs < 10)
                {
                    Console.WriteLine(string.Format("Diff at relative byte {0} (0x{0:X}): S1=0x{1:X2}, S2=0x{2:X2}",
                        i, s1[off1 + i], s2[off2 + i]));
                }
                diffs++;
            }
        }
        Console.WriteLine("Total byte differences: " + diffs);
    }
}
