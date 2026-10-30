using System;
using System.IO;

class Program
{
    static void Main()
    {
        string[] lines = File.ReadAllLines("Renew.cs");
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("Ensure") && lines[i].Contains("Capacity"))
            {
                Console.WriteLine($"Line {i+1}: {lines[i].Trim()}");
            }
        }
    }
}
