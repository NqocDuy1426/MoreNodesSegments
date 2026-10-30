using System;
using System.IO;
using System.Reflection;
using ColossalFramework.IO;

class Program
{
    static void Main(string[] args)
    {
        Check("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp");
        Check("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup3.crp");
    }

    static void Check(string path)
    {
        Console.WriteLine("=== Check: " + Path.GetFileName(path) + " ===");
        if (!File.Exists(path)) { Console.WriteLine("File not found!"); return; }
        using (var fs = File.OpenRead(path))
        {
            fs.Seek(370, SeekOrigin.Begin);
            var meta = DataSerializer.Deserialize<SimulationMetaData>(fs, (DataSerializer.Mode)0);
            Console.WriteLine("CityName: " + meta.m_CityName);
            Console.WriteLine("CurrentDateTime: " + meta.m_currentDateTime);
            Console.WriteLine("CurrentDayHour: " + meta.m_currentDayHour);
            Console.WriteLine("InGame: " + meta.m_InGame);
            Console.WriteLine("SimulationSpeed: " + meta.m_SimulationSpeed);
            Console.WriteLine("SimulationPaused: " + meta.m_SimulationPaused);
        }
    }
}
