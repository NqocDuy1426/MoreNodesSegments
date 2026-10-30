using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        var lines = File.ReadAllLines("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/net_des_full.txt");
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            if (line.Contains("elem"))
            {
                // Print line and the 3 preceding lines
                int start = Math.Max(0, i - 4);
                Console.WriteLine("Line " + (i + 1) + ": " + line.Trim());
                for (int j = start; j < i; j++)
                {
                    Console.WriteLine("    " + lines[j].Trim());
                }
                Console.WriteLine();
            }
        }
    }
}
