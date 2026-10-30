using System;
using System.IO;
using System.Collections.Generic;

public class CompareAllManagers
{
    public static void Main()
    {
        string p1 = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup .crp";
        string p2 = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";

        string[] mgrs = new string[] {
            "TerrainManager", "WaterSimulation", "TransferManager",
            "ElectricityManager", "WaterManager", "ZoneManager", "NetManager", "BuildingManager",
            "CitizenManager", "PropManager", "TreeManager", "WeatherManager",
            "VehicleManager", "TransportManager", "DistrictManager", "DisasterManager"
        };

        var m1 = ScanManagers(p1, mgrs);
        var m2 = ScanManagers(p2, mgrs);

        Console.WriteLine("\n=== Manager Chunk Comparison ===");
        Console.WriteLine(string.Format("{0,-20} | {1,12} | {2,12} | {3,12}", "Manager", "Save 1 Offset", "Save 2 Offset", "Offset Diff"));
        Console.WriteLine(new string('-', 65));

        foreach (var m in mgrs)
        {
            int off1 = m1.ContainsKey(m) ? m1[m] : -1;
            int off2 = m2.ContainsKey(m) ? m2[m] : -1;
            int diff = (off1 != -1 && off2 != -1) ? (off2 - off1) : 0;
            Console.WriteLine(string.Format("{0,-20} | {1,12} | {2,12} | {3,12}", m, off1, off2, diff));
        }
    }

    static Dictionary<string, int> ScanManagers(string path, string[] mgrs)
    {
        var result = new Dictionary<string, int>();
        byte[] all = File.ReadAllBytes(path);
        foreach (var m in mgrs)
        {
            byte[] pat = EncodeString(m);
            for (int i = 10000; i <= all.Length - pat.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < pat.Length; j++)
                {
                    if (all[i + j] != pat[j]) { match = false; break; }
                }
                if (match)
                {
                    result[m] = i;
                    break;
                }
            }
        }
        return result;
    }

    static byte[] EncodeString(string str)
    {
        List<byte> bytes = new List<byte>();
        int running = 128;
        foreach (char c in str)
        {
            int diff = ((int)c - running) & 0xFF;
            bytes.Add((byte)diff);
            running = (int)c;
        }
        return bytes.ToArray();
    }
}
