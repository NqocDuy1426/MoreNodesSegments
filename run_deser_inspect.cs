
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
        try
        {
            string savePath = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/backup12.crp";
            using (var fs = File.OpenRead(savePath))
            {
                fs.Seek(316, SeekOrigin.Begin);
                var meta = DataSerializer.Deserialize<SimulationMetaData>(fs, DataSerializer.Mode.File);
                
                var ctors = typeof(DataSerializer).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
                ConstructorInfo ctor = null;
                foreach (var c in ctors) {
                    if (c.GetParameters().Length == 3) { ctor = c; break; }
                }
                var ds = (DataSerializer)ctor.Invoke(new object[] { fs, DataSerializer.Mode.File, false });
                
                uint v1 = ds.ReadUInt16();
                uint v2 = ds.ReadUInt16();
                var fiVersion = typeof(DataSerializer).GetField("m_version", BindingFlags.NonPublic | BindingFlags.Instance);
                fiVersion.SetValue(ds, v2);
                
                var root = ds.ReadObject<SimulationManager.Data>();
                
                ds.ReadUInt32(); // currentTickIndex
                ds.ReadUInt32(); // currentFrameIndex
                ds.ReadUInt32(); // currentBuildIndex
                ds.ReadULong64(); // seed
                ds.ReadUInt32(); // dayTimeOffsetFrames
                
                for (int i = 0; i < 32; i++) ds.ReadUInt32();
                
                Console.WriteLine("Reading IDataContainer[] array at position " + fs.Position);
                MethodInfo mRead = null;
                foreach (var m in typeof(DataSerializer).GetMethods(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (m.Name == "ReadObjectArray" && m.GetParameters().Length == 0 && m.IsGenericMethod)
                    {
                        mRead = m;
                        break;
                    }
                }
                var mReadArray = mRead.MakeGenericMethod(typeof(IDataContainer));
                var arr = (IDataContainer[])mReadArray.Invoke(ds, null);
                Console.WriteLine("Read IDataContainer[] array of length: " + arr.Length);
                for (int i = 0; i < arr.Length; i++)
                {
                    Console.WriteLine(string.Format("  [{0}] {1}", i, arr[i]?.GetType().FullName));
                }
                
                var fiList = typeof(DataSerializer).GetField("m_objectList", BindingFlags.NonPublic | BindingFlags.Instance);
                var objList = (List<IDataContainer>)fiList.GetValue(ds);
                Console.WriteLine("Total m_objectList count: " + objList.Count);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Caught exception: " + ex);
        }
    }
}
