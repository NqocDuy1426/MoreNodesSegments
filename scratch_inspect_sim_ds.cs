using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using ColossalFramework.IO;

class Program
{
    static void Main()
    {
        using (var fs = File.OpenRead("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/backup4_sim.bin"))
        {
            Console.WriteLine("Step 1: Deserialize SimulationMetaData");
            var meta = DataSerializer.Deserialize<SimulationMetaData>(fs, DataSerializer.Mode.Memory);
            Console.WriteLine("Stream pos after meta: " + fs.Position);

            Console.WriteLine("Step 2: Inspect second DataSerializer header on stream...");
            // Let's create DataSerializer manually or inspect how it reads header
            var ctors = typeof(DataSerializer).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            ConstructorInfo targetCtor = null;
            foreach (var c in ctors)
            {
                var p = c.GetParameters();
                if (p.Length == 3 && p[0].ParameterType == typeof(Stream))
                {
                    targetCtor = c;
                    break;
                }
            }
            var ds = (DataSerializer)targetCtor.Invoke(new object[] { fs, DataSerializer.Mode.Memory, false });
            
            // Call DeserializeImplementation or read header
            // Let's check private methods in DataSerializer
            var methods = typeof(DataSerializer).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var m in methods)
            {
                if (m.Name.Contains("Header") || m.Name.Contains("Init") || m.Name.Contains("Deserialize"))
                {
                    Console.WriteLine("  Method: " + m.Name);
                }
            }
        }
    }
}
