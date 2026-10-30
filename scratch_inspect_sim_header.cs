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
            var ctors = typeof(DataSerializer).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            DataSerializer ds = null;
            foreach (var c in ctors)
            {
                var p = c.GetParameters();
                if (p.Length == 3 && p[0].ParameterType == typeof(Stream))
                {
                    ds = (DataSerializer)c.Invoke(new object[] { fs, DataSerializer.Mode.File, false });
                    break;
                }
            }
            Console.WriteLine("DataSerializer created! Version: " + ds.version);

            var meta = ds.ReadObject<SimulationMetaData>();
            Console.WriteLine("SimulationMetaData read! Stream position: " + fs.Position);

            var stringsField = typeof(DataSerializer).GetField("m_SharedStrings", BindingFlags.NonPublic | BindingFlags.Instance);
            if (stringsField != null)
            {
                var strings = (List<string>)stringsField.GetValue(ds);
                Console.WriteLine("Shared strings count: " + (strings != null ? strings.Count : -1));
                if (strings != null)
                {
                    for (int i = 0; i < Math.Min(100, strings.Count); i++)
                        Console.WriteLine("  [" + i + "] " + strings[i]);
                }
            }
        }
    }
}
