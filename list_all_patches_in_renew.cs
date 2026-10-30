using System;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string[] lines = File.ReadAllLines("Renew.cs");
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("harmony.Patch(") || lines[i].Contains(".Patch("))
            {
                Console.WriteLine($"Line {i+1}: {lines[i].Trim()}");
                // print preceding 2 lines
                for (int j = Math.Max(0, i - 2); j < i; j++)
                    Console.WriteLine($"    {lines[j].Trim()}");
            }
        }
    }
}
