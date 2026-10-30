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
            var meta = DataSerializer.Deserialize<SimulationMetaData>(fs, DataSerializer.Mode.Memory);
            Console.WriteLine("Meta read, environment=" + meta.m_environment + ", version=" + meta.m_updateMode);

            // Construct DataSerializer with stream
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
            
            // Read version (DataSerializer ctor with readHeader=true already read version)
            Console.WriteLine("DataSerializer version: " + ds.version);

            // SimulationManager.Data
            var smData = ds.ReadObject<SimulationManager.Data>();
            Console.WriteLine("ReadObject<SimulationManager.Data> done. Position: " + fs.Position);

            if (ds.version >= 84) ds.ReadUInt32(); // currentTickIndex
            if (ds.version >= 5) ds.ReadUInt32(); // currentFrameIndex
            if (ds.version >= 13) ds.ReadUInt32(); // currentBuildIndex
            if (ds.version >= 10) ds.ReadULong64(); // seed
            if (ds.version >= 212) ds.ReadUInt32(); // dayTimeOffsetFrames
            if (ds.version >= 20 && ds.version < 183) ds.ReadLong64(); // dateTime
            if (ds.version >= 164)
            {
                for (int i = 0; i < 32; i++) ds.ReadUInt32();
            }

            Console.WriteLine("Position before ReadObjectArray: " + fs.Position);
            var containers = ds.ReadObjectArray<IDataContainer>();
            Console.WriteLine("Containers length: " + (containers != null ? containers.Length : -1));
            for (int i = 0; i < containers.Length; i++)
            {
                Console.WriteLine($"  [{i}] {containers[i].GetType().FullName}");
            }
        }
    }
}
