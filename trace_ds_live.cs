using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using ColossalFramework.IO;

public class TraceDSLive
{
    public static void Main()
    {
        string savePath = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        using (FileStream fs = new FileStream(savePath, FileMode.Open, FileAccess.Read))
        {
            fs.Seek(370, SeekOrigin.Begin);
            var meta = DataSerializer.Deserialize<SimulationMetaData>(fs, DataSerializer.Mode.Memory);
            Console.WriteLine("Meta OK. Pos = " + fs.Position);

            // Create DataSerializer for SimulationManager.Data
            var ctor = typeof(DataSerializer).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Stream), typeof(DataSerializer.Mode), typeof(bool) }, null);
            DataSerializer ds = (DataSerializer)ctor.Invoke(new object[] { fs, DataSerializer.Mode.Memory, false });
            
            // First read the version
            uint v0 = ds.ReadUInt16();
            uint v1 = ds.ReadUInt16();
            uint version = (v0 << 16) | v1;
            if (v1 <= 322 && v0 <= 6) version = v1;
            
            var fVer = typeof(DataSerializer).GetField("m_version", BindingFlags.NonPublic | BindingFlags.Instance);
            fVer.SetValue(ds, version);
            Console.WriteLine(string.Format("SimManager version = {0}, pos = {1}", version, fs.Position));

            // Read root object (SimulationManager.Data header)
            var readObjMethod = typeof(DataSerializer).GetMethod("ReadObject", BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(DataSerializer.LegacyResolverHandler) }, null);
            var readObjGeneric = readObjMethod.MakeGenericMethod(typeof(IDataContainer));
            var rootObj = readObjGeneric.Invoke(ds, new object[] { (DataSerializer.LegacyResolverHandler)((name) => Type.GetType(name)) });
            Console.WriteLine("Root obj type: " + rootObj.GetType().FullName + ", pos = " + fs.Position);

            // Now read SimulationManager.Data fields:
            ds.ReadUInt32(); // tickIndex
            ds.ReadUInt32(); // frameIndex
            ds.ReadUInt32(); // buildIndex
            ds.ReadULong64(); // seed
            ds.ReadUInt32(); // dayTimeOffset
            for (int i = 0; i < 32; i++) ds.ReadUInt32();

            Console.WriteLine("Before ReadObjectArray<IDataContainer>: pos = " + fs.Position);
            
            var readObjArrMethod = typeof(DataSerializer).GetMethod("ReadObjectArray", BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(DataSerializer.LegacyResolverHandler) }, null);
            var readObjArrGeneric = readObjArrMethod.MakeGenericMethod(typeof(IDataContainer));

            var containers = (IDataContainer[])readObjArrGeneric.Invoke(ds, new object[] { (DataSerializer.LegacyResolverHandler)((name) => Type.GetType(name)) });
            Console.WriteLine(string.Format("Containers count = {0}, pos = {1}", containers.Length, fs.Position));

            var fObjList = typeof(DataSerializer).GetField("m_objectList", BindingFlags.NonPublic | BindingFlags.Instance);
            var objList = (List<IDataContainer>)fObjList.GetValue(ds);
            Console.WriteLine("Total objects registered in m_objectList: " + objList.Count);

            for (int i = 0; i < objList.Count; i++)
            {
                var o = objList[i];
                Console.WriteLine(string.Format("  [{0}] {1}", i, o != null ? o.GetType().Name : "null"));
            }
        }
    }
}
