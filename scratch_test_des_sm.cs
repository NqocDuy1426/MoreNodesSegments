using System;
using System.IO;
using ColossalFramework.IO;

class Program {
    static void Main() {
        using (var fs = File.OpenRead("backup4_sim.bin")) {
            var meta = DataSerializer.Deserialize<SimulationMetaData>(fs, DataSerializer.Mode.Memory);
            Console.WriteLine("Meta deserialized, stream pos=" + fs.Position);
            try {
                var smData = DataSerializer.Deserialize<SimulationManager.Data>(fs, DataSerializer.Mode.Memory);
                Console.WriteLine("SM data deserialized successfully!");
            } catch (Exception ex) {
                Console.WriteLine("SM deserialize exception: " + ex.GetType().FullName + " -> " + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
