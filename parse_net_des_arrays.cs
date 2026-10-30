using System;
using System.IO;
using System.Collections.Generic;

class ParseArrays
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
                Console.WriteLine(string.Format("Line {0,4}: {1} | {2} | {3}", i + 1, prev2.Trim(), prev.Trim(), l.Trim()));
            }
            prev2 = prev;
            prev = l;
        }
    }
}
