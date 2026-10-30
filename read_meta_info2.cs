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
            foreach (var f in typeof(SimulationMetaData).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                object val = f.GetValue(meta);
                Console.WriteLine("  " + f.Name + " = " + val);
            }
        }
    }
}
