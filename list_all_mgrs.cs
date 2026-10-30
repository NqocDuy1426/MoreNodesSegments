using System;
using System.IO;
using System.Collections.Generic;

public class ListAllManagers
{
    public static void Main()
    {
        string p2 = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        string[] mgrs = new string[] {
            "SimulationManager", "WeatherManager", "TerrainManager", "WaterManager",
            "DistrictManager", "ZoneManager", "NetManager", "BuildingManager",
            "PropManager", "TreeManager", "CitizenManager", "VehicleManager",
            "DisasterManager", "ElectricityManager", "WaterSimulation", "TransportManager"
        };

        foreach (var m in mgrs)
        {
            byte[] pat = EncodeString(m);
            Scan(p2, m, pat);
        }
    }

    static void Scan(string path, string name, byte[] pattern)
    {
        byte[] all = File.ReadAllBytes(path);
        for (int i = 0; i <= all.Length - pattern.Length; i++)
        {
            bool match = true;
            for (int j = 0; j < pattern.Length; j++)
            {
                if (all[i + j] != pattern[j]) { match = false; break; }
            }
            if (match && i > 10000)
            {
                Console.WriteLine(string.Format("{0,-20} at offset {1}", name, i));
            }
        }
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
