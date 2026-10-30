using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class ScanSaveStrings
{
    public static void Main()
    {
        string savesDir = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves";
        string save1 = Path.Combine(savesDir, "QuangNinh backup .crp");
        string save2 = Path.Combine(savesDir, "QuangNinh backup2.crp");

        Scan(save1);
        Console.WriteLine("\n--------------------------------------------------\n");
        Scan(save2);
    }

    static void Scan(string path)
    {
        Console.WriteLine("Scanning: " + Path.GetFileName(path));
        byte[] bytes = File.ReadAllBytes(path);

        string[] targets = new string[] {
            "TransportManager", "TransportLine", "SimulationManager", "VehicleManager",
            "Bus", "Line", "CustomPathFind", "TrafficManager"
        };

        foreach (var t in targets)
        {
            byte[] targetBytes = Encoding.UTF8.GetBytes(t);
            int count = 0;
            int pos = 0;
            while ((pos = IndexOf(bytes, targetBytes, pos)) != -1)
            {
                count++;
                pos += targetBytes.Length;
            }
            Console.WriteLine(string.Format("  '{0}': {1} occurrences", t, count));
        }
    }

    static int IndexOf(byte[] array, byte[] pattern, int start)
    {
        for (int i = start; i <= array.Length - pattern.Length; i++)
        {
            bool match = true;
            for (int j = 0; j < pattern.Length; j++)
            {
                if (array[i + j] != pattern[j])
                {
                    match = false;
                    break;
                }
            }
            if (match) return i;
        }
        return -1;
    }
}
