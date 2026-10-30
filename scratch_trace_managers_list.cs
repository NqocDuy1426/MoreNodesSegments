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
            
            uint curFrame = ds.ReadUInt32();
            uint curFrame2 = ds.ReadUInt32();
            uint curFrame3 = ds.ReadUInt32();
            ulong curTime = ds.ReadULong64();
            uint curBuild = ds.ReadUInt32();
            
            // version >= 164: read 32 uint32s
            for (int i = 0; i < 32; i++)
            {
                ds.ReadUInt32();
            }

            Console.WriteLine("Reading manager array at pos " + fs.Position);
            var managers = ds.ReadObjectArray<IDataContainer>();
            Console.WriteLine("Managers array length: " + (managers != null ? managers.Length : -1));
            if (managers != null)
            {
                for (int i = 0; i < managers.Length; i++)
                {
                    Console.WriteLine("  [" + i + "] " + (managers[i] != null ? managers[i].GetType().FullName : "null"));
                }
            }
        }
    }
}
