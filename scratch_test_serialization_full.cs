using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        string tmpeDll = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        string gameDir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        
        Assembly.LoadFrom(Path.Combine(gameDir, "Assembly-CSharp.dll"));
        Assembly.LoadFrom(Path.Combine(gameDir, "ColossalManaged.dll"));
        Assembly.LoadFrom(Path.Combine(gameDir, "ICities.dll"));
        Assembly.LoadFrom(Path.Combine(gameDir, "UnityEngine.dll"));
        Assembly tmpe = Assembly.LoadFrom(tmpeDll);

        Type ttlCfgType = tmpe.GetType("TrafficManager.Configuration+TimedTrafficLights");
        Type stepCfgType = tmpe.GetType("TrafficManager.Configuration+TimedTrafficLightsStep");
        Type cslCfgType = tmpe.GetType("TrafficManager.Configuration+CustomSegmentLights");
        Type lightCfgType = tmpe.GetType("TrafficManager.Configuration+CustomSegmentLight");

        // Create a mock TimedTrafficLights config object
        object ttlObj = Activator.CreateInstance(ttlCfgType);
        ttlCfgType.GetField("nodeId").SetValue(ttlObj, (ushort)40000);
        ttlCfgType.GetField("nodeGroup").SetValue(ttlObj, new List<ushort> { 40000, 100 });
        ttlCfgType.GetField("started").SetValue(ttlObj, true);
        ttlCfgType.GetField("currentStep").SetValue(ttlObj, 0);

        Type stepListType = typeof(List<>).MakeGenericType(stepCfgType);
        IList stepList = (IList)Activator.CreateInstance(stepListType);
        ttlCfgType.GetField("timedSteps").SetValue(ttlObj, stepList);

        object stepObj = Activator.CreateInstance(stepCfgType);
        stepCfgType.GetField("minTime").SetValue(stepObj, 2);
        stepCfgType.GetField("maxTime").SetValue(stepObj, 15);
        stepCfgType.GetField("changeMetric").SetValue(stepObj, 0);
        stepCfgType.GetField("waitFlowBalance").SetValue(stepObj, 1f);

        Type cslDictType = typeof(Dictionary<,>).MakeGenericType(typeof(ushort), cslCfgType);
        IDictionary cslDict = (IDictionary)Activator.CreateInstance(cslDictType);
        stepCfgType.GetField("segmentLights").SetValue(stepObj, cslDict);

        object cslObj = Activator.CreateInstance(cslCfgType);
        cslCfgType.GetField("nodeId").SetValue(cslObj, (ushort)40000);
        cslCfgType.GetField("segmentId").SetValue(cslObj, (ushort)50000);
        cslCfgType.GetField("manualPedestrianMode").SetValue(cslObj, false);

        Type lightDictType = cslCfgType.GetField("customLights").FieldType;
        IDictionary lightDict = (IDictionary)Activator.CreateInstance(lightDictType);
        cslCfgType.GetField("customLights").SetValue(cslObj, lightDict);

        object lightObj = Activator.CreateInstance(lightCfgType);
        lightCfgType.GetField("nodeId").SetValue(lightObj, (ushort)40000);
        lightCfgType.GetField("segmentId").SetValue(lightObj, (ushort)50000);
        lightCfgType.GetField("currentMode").SetValue(lightObj, 0);

        Type extVehType = tmpe.GetType("TrafficManager.Traffic.ExtVehicleType");
        object roadVeh = Enum.Parse(extVehType, "RoadVehicle");
        lightDict.Add(roadVeh, lightObj);
        cslDict.Add((ushort)50000, cslObj);
        stepList.Add(stepObj);

        Type listType = typeof(List<>).MakeGenericType(ttlCfgType);
        IList ttlList = (IList)Activator.CreateInstance(listType);
        ttlList.Add(ttlObj);

        // Test BinaryFormatter serialization & deserialization
        byte[] bytes;
        using (MemoryStream ms = new MemoryStream())
        {
            var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            bf.Serialize(ms, ttlList);
            bytes = ms.ToArray();
        }

        Console.WriteLine("Serialized TTL bytes: " + bytes.Length);

        using (MemoryStream ms = new MemoryStream(bytes))
        {
            var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            IList deserList = (IList)bf.Deserialize(ms);
            Console.WriteLine("Deserialized TTL count: " + deserList.Count);
            object first = deserList[0];
            ushort nId = (ushort)ttlCfgType.GetField("nodeId").GetValue(first);
            Console.WriteLine("Deserialized node ID: " + nId);
            List<ushort> group = (List<ushort>)ttlCfgType.GetField("nodeGroup").GetValue(first);
            Console.WriteLine("Deserialized group: " + string.Join(", ", group.ConvertAll(x => x.ToString()).ToArray()));
        }

        Console.WriteLine("SUCCESS!");
    }
}
