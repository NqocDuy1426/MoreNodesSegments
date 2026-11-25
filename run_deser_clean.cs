using System;
using System.IO;
using System.Reflection;
using ColossalFramework.IO;

class Program
{
    static void Main()
    {
        try
        {
            string savePath = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/backup12.crp";
            using (var fs = File.OpenRead(savePath))
            {
                fs.Seek(316, SeekOrigin.Begin);
                var meta = DataSerializer.Deserialize<SimulationMetaData>(fs, DataSerializer.Mode.File);
                Console.WriteLine("Meta deserialized! Sim data stream starts at: " + fs.Position);
                
                // Let's create DataSerializer
                var smData = DataSerializer.Deserialize<SimulationManager.Data>(fs, DataSerializer.Mode.File);
                Console.WriteLine("SimulationManager.Data deserialized successfully! Stream pos: " + fs.Position);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Caught exception: " + ex);
        }
    }
}
