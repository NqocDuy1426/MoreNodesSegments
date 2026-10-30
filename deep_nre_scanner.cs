using System;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string[] lines = File.ReadAllLines("Renew.cs");
        int count = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();

            // Check for unsafe cast: (Type)var.Field or (as Type).Field
            if (Regex.IsMatch(line, @"\([A-Za-z0-9_]+\s+as\s+[A-Za-z0-9_]+\)\.[A-Za-z0-9_]+"))
            {
                Console.WriteLine($"[Line {i+1}] (x as T).field without null check: {line}");
                count++;
            }

            // Check Singleton<T>.instance.Field without check
            if (line.Contains("Singleton<") && line.Contains(">.instance.") && !line.Contains("!= null") && !line.Contains("?."))
            {
                // check if the line itself or previous line checks it
                if (!line.Contains("if (Singleton<") && !line.Contains("instance != null"))
                {
                    Console.WriteLine($"[Line {i+1}] Direct Singleton dereference: {line}");
                    count++;
                }
            }

            // Check for unhandled exceptions: methods that don't have try/catch where exceptions can crash threads
            // Especially in Harmony prefixes/postfixes
        }
        Console.WriteLine($"Total suspicious lines found: {count}");
    }
}
