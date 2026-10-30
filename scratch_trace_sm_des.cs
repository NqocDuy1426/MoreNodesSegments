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
            var meta = DataSerializer.Deserialize<SimulationMetaData>(fs, DataSerializer.Mode.Memory);

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
            
            uint v1 = ds.ReadUInt16();
            uint v2 = ds.ReadUInt16();
            typeof(DataSerializer).GetField("m_version", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ds, v2);
            typeof(DataSerializer).GetField("m_altVersion", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ds, v1);

            var smData = ds.ReadObject<SimulationManager.Data>();
            
            // Now simulate SimulationManager.Data.Deserialize:
            // Skip the string tag or let's see how BeginDeserialize works
            Console.WriteLine("DataSerializer version: " + ds.version);
            
            // In SimulationManager.Data.Deserialize:
            // version >= 10: ReadUInt32() (currentFrameIndex)
            uint curFrame = ds.ReadUInt32();
            uint curFrame2 = ds.ReadUInt32();
            uint curFrame3 = ds.ReadUInt32();
            ulong curTime = ds.ReadULong64();
            uint curBuild = ds.ReadUInt32();
            Console.WriteLine("curFrame: " + curFrame + ", curBuild: " + curBuild + " stream pos: " + fs.Position);

            // DateTime ticks
            long t1 = ds.ReadLong64();
            long t2 = ds.ReadLong64();
            uint rand1 = ds.ReadUInt32();

            // buildIndexHistory (64 items)
            for (int i = 0; i < 64; i++)
            {
                // wait, in SM.Data.Deserialize does it read buildIndexHistory?
                // Let's check!
            }
            Console.WriteLine("Stream pos before managers: " + fs.Position);
        }
    }
}
