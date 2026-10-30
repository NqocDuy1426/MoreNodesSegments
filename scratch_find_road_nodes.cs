using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using ColossalFramework.IO;
using ColossalFramework.Packaging;

class Program
{
    static void Main()
    {
        string savePath = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        Console.WriteLine("Inspecting: " + savePath);
        using (FileStream fs = File.OpenRead(savePath))
        {
            var pkg = new Package(null, savePath);
            Package.Asset dataAsset = null;
            foreach (Package.Asset a in pkg)
            {
                if (a.name != null && a.name.EndsWith("_Data")) { dataAsset = a; break; }
            }
            if (dataAsset == null) { Console.WriteLine("No data asset!"); return; }

            using (Stream s = dataAsset.GetStream())
            {
                var dsCtor = typeof(DataSerializer).GetConstructor(
                    BindingFlags.NonPublic | BindingFlags.Instance, null,
                    new Type[] { typeof(Stream), typeof(DataSerializer.Mode), typeof(bool) }, null);
                DataSerializer ds = (DataSerializer)dsCtor.Invoke(new object[] { s, DataSerializer.Mode.File, false });

                // Read SimulationMetaData
                var meta = DataSerializer.Deserialize<SimulationMetaData>(s, DataSerializer.Mode.File);
                Console.WriteLine("CityName: " + meta.m_CityName);

                // Let's inspect NetManager data:
                // NetManager saves road names in m_segmentNames (or string table)
                // Let's find NetManager in the stream!
            }
        }
    }
}
