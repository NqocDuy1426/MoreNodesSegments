using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        string[] lines = File.ReadAllLines("Renew.cs");
        Console.WriteLine($"Total lines: {lines.Length}");

        // Scan for potential array index issues: arr[expr]
        // where arr is not checked for null or bounds
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (line.StartsWith("//") || line.StartsWith("*")) continue;

            // Check for .m_buffer[
            var match = Regex.Match(line, @"([A-Za-z0-9_\.]+)\.m_buffer\[([^\]]+)\]");
            if (match.Success)
            {
                string target = match.Groups[1].Value;
                string indexExpr = match.Groups[2].Value.Trim();

                // Check if line or previous 5 lines checked target != null and target.m_buffer != null
                bool checkedNull = false;
                for (int j = Math.Max(0, i - 15); j <= i; j++)
                {
                    if (lines[j].Contains(target + " != null") || lines[j].Contains("m_buffer != null") || lines[j].Contains("m_buffer == null") || lines[j].Contains("try"))
                    {
                        checkedNull = true;
                        break;
                    }
                }
                if (!checkedNull)
                {
                    Console.WriteLine($"Line {i+1}: Unprotected buffer access: {line}");
                }
            }
        }
    }
}
