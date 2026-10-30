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
            Console.WriteLine("Offset after meta: " + fs.Position);

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
            
            // Read header
            uint v1 = ds.ReadUInt16();
            uint v2 = ds.ReadUInt16();
            uint ver = (v1 << 16) | v2;
            Console.WriteLine("v1=" + v1 + ", v2=" + v2 + ", ver=" + ver);

            // Set version fields
            typeof(DataSerializer).GetField("m_version", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ds, v2);
            typeof(DataSerializer).GetField("m_altVersion", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ds, v1);

            Console.WriteLine("Reading shared strings count...");
            // Let's see what ReadObject<SimulationManager.Data> does
            var objListField = typeof(DataSerializer).GetField("m_objectList", BindingFlags.NonPublic | BindingFlags.Instance);
            
            // Read root object
            var smData = ds.ReadObject<SimulationManager.Data>();
            var objList = (List<IDataContainer>)objListField.GetValue(ds);
            Console.WriteLine("Root read! m_objectList count: " + objList.Count);
            foreach (var obj in objList)
            {
                Console.WriteLine("  Obj: " + obj.GetType().FullName);
            }
        }
    }
}
