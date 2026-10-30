using System;
using System.IO;

class ParseOther
{
    static void Main()
    {
        string[] lines = File.ReadAllLines("net_des_full.txt");
        string prev = "";
        string prev2 = "";
        for (int i = 0; i < lines.Length; i++)
        {
            string l = lines[i];
            if (l.Contains("elem"))
            {
                if (!prev2.Contains("ldloc.1") && !prev2.Contains("ldloc.2") && !prev2.Contains("ldloc.3"))
                {
                    Console.WriteLine(string.Format("Line {0,4}: {1} | {2} | {3}", i + 1, prev2.Trim(), prev.Trim(), l.Trim()));
                }
            }
            prev2 = prev;
            prev = l;
        }
    }
}
