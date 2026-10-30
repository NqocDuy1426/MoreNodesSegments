using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using ColossalFramework.IO;

class Program
{
    static void Main()
    {
        using (var fs = File.OpenRead("backup4_sim.bin"))
        {
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
            var ds = (DataSerializer)targetCtor.Invoke(new object[] { fs, DataSerializer.Mode.Memory, true });
            var meta = ds.ReadObject<SimulationMetaData>();

            var ds2 = (DataSerializer)targetCtor.Invoke(new object[] { fs, DataSerializer.Mode.Memory, true });
            
            // Read SimulationManager.Data header
            // Read version (already read in ctor)
            // Call ReadObject<SimulationManager.Data>()
            // Note: ReadObject only instantiates the object and adds to m_objectList, it DOES NOT call Deserialize!
            var smData = ds2.ReadObject<SimulationManager.Data>();
            
            // Read frame, seed, build history
            ds2.ReadUInt32(); // tickIndex
            ds2.ReadUInt32(); // frameIndex
            ds2.ReadUInt32(); // buildIndex
            ds2.ReadULong64(); // seed
            ds2.ReadUInt32(); // dayTimeOffsetFrames
            for (int i = 0; i < 32; i++) ds2.ReadUInt32(); // buildIndexHistory

            Console.WriteLine("Now reading ObjectArray<IDataContainer> at stream pos: " + fs.Position);
            var containers = ds2.ReadObjectArray<IDataContainer>();
            Console.WriteLine("Containers count: " + containers.Length);
            for (int i = 0; i < containers.Length; i++)
            {
                Console.WriteLine($"  [{i}] {containers[i].GetType().FullName}");
            }
        }
    }
}
