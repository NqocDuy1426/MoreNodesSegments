using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ColossalFramework.Packaging;
using ColossalFramework.IO;

class PrintContainers
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        var pkg = new Package(null, path);
        var field = typeof(Package).GetField("m_IndexTable", BindingFlags.NonPublic | BindingFlags.Instance);
        var dict = (IDictionary)field.GetValue(pkg);
        foreach (DictionaryEntry entry in dict)
        {
            Package.Asset asset = (Package.Asset)entry.Value;
            if (asset.name != null && asset.name.EndsWith("_Data"))
            {
                using (var s = asset.GetStream())
                {
                    var meta = DataSerializer.Deserialize<SimulationMetaData>(s, DataSerializer.Mode.File);
                    var ctor = typeof(DataSerializer).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Stream), typeof(DataSerializer.Mode), typeof(bool) }, null);
                    var ds = (DataSerializer)ctor.Invoke(new object[] { s, DataSerializer.Mode.File, false });

                    // Header
                    MethodInfo mReadU16 = typeof(DataSerializer).GetMethod("ReadUInt16");
                    uint v1 = (uint)mReadU16.Invoke(ds, null);
                    uint v2 = (uint)mReadU16.Invoke(ds, null);
                    uint ver = (v1 << 16) | v2;
                    if (v2 <= 322 && v1 <= 6)
                    {
                        typeof(DataSerializer).GetField("m_altVersion", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ds, v1);
                        typeof(DataSerializer).GetField("m_version", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ds, v2);
                    }
                    else if ((v1 & 32768) != 0)
                    {
                        MethodInfo mReadU32 = typeof(DataSerializer).GetMethod("ReadUInt32");
                        uint alt = (uint)mReadU32.Invoke(ds, null);
                        typeof(DataSerializer).GetField("m_altVersion", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ds, alt);
                        typeof(DataSerializer).GetField("m_version", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ds, ver & 0x7FFFFFFF);
                    }
                    else
                    {
                        typeof(DataSerializer).GetField("m_altVersion", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ds, 0u);
                        typeof(DataSerializer).GetField("m_version", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ds, ver);
                    }

                    // Read SimulationManager.Data fields:
                    ds.ReadUInt32(); // tickIndex
                    ds.ReadUInt32(); // frameIndex
                    ds.ReadUInt32(); // buildIndex
                    ds.ReadULong64(); // seed
                    ds.ReadUInt32(); // dayTimeOffset
                    for (int i = 0; i < 32; i++) ds.ReadUInt32(); // buildIndexHistory

                    Console.WriteLine("Before ReadObjectArray, stream pos=" + s.Position);
                    // ReadObjectArray<IDataContainer>
                    MethodInfo mReadObjArray = typeof(DataSerializer).GetMethod("ReadObjectArray", new Type[] { typeof(DataSerializer.LegacyResolverHandler) }).MakeGenericMethod(typeof(IDataContainer));
                    IDataContainer[] containers = (IDataContainer[])mReadObjArray.Invoke(ds, new object[] { null });
                    Console.WriteLine("After ReadObjectArray, stream pos=" + s.Position);
                    Console.WriteLine("Total containers: " + containers.Length);
                    for (int i = 0; i < containers.Length; i++)
                    {
                        Console.WriteLine(string.Format("Container [{0}]: {1}", i, containers[i].GetType().FullName));
                    }
                }
            }
        }
    }
}
