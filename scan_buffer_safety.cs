using System;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string[] lines = File.ReadAllLines("Renew.cs");
        string[] managerFields = new string[] {
            "m_nodes",
            "m_segments",
            "m_lanes",
            "m_vehicles",
            "m_parkedVehicles",
            "m_citizens",
            "m_units",
            "m_buildings",
            "m_districts",
            "m_lines"
        };

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (line.StartsWith("//")) continue;

            foreach (var fld in managerFields)
            {
                if (line.Contains("." + fld + ".m_buffer["))
                {
                    // Check if previous 10 lines checked fld != null or fld.m_buffer != null
                    bool ok = false;
                    for (int j = Math.Max(0, i - 12); j <= i; j++)
                    {
                        if (lines[j].Contains(fld + " != null") || lines[j].Contains("m_buffer != null") || lines[j].Contains("try") || lines[j].Contains("if (") && lines[j].Contains(fld))
                        {
                            ok = true;
                            break;
                        }
                    }
                    if (!ok)
                    {
                        Console.WriteLine($"[Line {i+1}] {line}");
                    }
                }
            }
        }
    }
}
