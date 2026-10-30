using System;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string[] lines = File.ReadAllLines("Renew.cs");
        Console.WriteLine($"Total lines in Renew.cs: {lines.Length}");

        // Check for risky patterns
        // 1. Singleton<...>.instance.FIELD without preceding null check
        // 2. .Info. without preceding null check
        // 3. .m_buffer[ without bounds check
        // 4. Reflection .Invoke without null check

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            // Pattern: .Info.something
            if (line.Contains(".Info.") && !line.Contains("!= null") && !line.Contains("?."))
            {
                // Check if previous lines check Info != null
                bool checkedNull = false;
                for (int j = Math.Max(0, i - 5); j < i; j++)
                {
                    if (lines[j].Contains("Info != null") || lines[j].Contains("Info == null"))
                    {
                        checkedNull = true;
                        break;
                    }
                }
                if (!checkedNull)
                {
                    Console.WriteLine($"[Line {i + 1}] Potential Info null dereference: {line.Trim()}");
                }
            }

            // Pattern: .m_netAI.
            if (line.Contains(".m_netAI.") && !line.Contains("!= null"))
            {
                bool checkedNull = false;
                for (int j = Math.Max(0, i - 5); j < i; j++)
                {
                    if (lines[j].Contains("m_netAI != null") || lines[j].Contains("m_netAI == null"))
                    {
                        checkedNull = true;
                        break;
                    }
                }
                if (!checkedNull)
                {
                    Console.WriteLine($"[Line {i + 1}] Potential m_netAI null dereference: {line.Trim()}");
                }
            }
        }
    }
}
