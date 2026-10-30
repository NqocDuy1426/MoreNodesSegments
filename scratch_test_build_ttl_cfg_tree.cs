using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        string dir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/";
        Assembly tmpe = Assembly.LoadFrom(dir + "TrafficManager.dll");
        Type ttlCfgType = tmpe.GetType("TrafficManager.Configuration+TimedTrafficLights");
        Type stepCfgType = tmpe.GetType("TrafficManager.Configuration+TimedTrafficLightsStep");
        Type cslCfgType = tmpe.GetType("TrafficManager.Configuration+CustomSegmentLights");
        Type cslLightCfgType = tmpe.GetType("TrafficManager.Configuration+CustomSegmentLight");

        // 1. Create TTL cfg
        object ttlObj = Activator.CreateInstance(ttlCfgType);
        ttlCfgType.GetField("nodeId").SetValue(ttlObj, (ushort)40000);
        ttlCfgType.GetField("nodeGroup").SetValue(ttlObj, new List<ushort>() { 40000 });
        ttlCfgType.GetField("started").SetValue(ttlObj, true);
        ttlCfgType.GetField("currentStep").SetValue(ttlObj, 0);

        Type stepListType = typeof(List<>).MakeGenericType(stepCfgType);
        IList stepList = (IList)Activator.CreateInstance(stepListType);
        ttlCfgType.GetField("timedSteps").SetValue(ttlObj, stepList);

        // 2. Create Step cfg
        object stepObj = Activator.CreateInstance(stepCfgType);
        stepCfgType.GetField("minTime").SetValue(stepObj, 2);
        stepCfgType.GetField("maxTime").SetValue(stepObj, 10);
        stepCfgType.GetField("changeMetric").SetValue(stepObj, 0);
        stepCfgType.GetField("waitFlowBalance").SetValue(stepObj, 1.0f);

        Type cslDictType = typeof(Dictionary<,>).MakeGenericType(typeof(ushort), cslCfgType);
        IDictionary cslDict = (IDictionary)Activator.CreateInstance(cslDictType);
        stepCfgType.GetField("segmentLights").SetValue(stepObj, cslDict);
        stepList.Add(stepObj);

        // 3. Create CSL cfg
        object cslObj = Activator.CreateInstance(cslCfgType);
        cslCfgType.GetField("nodeId").SetValue(cslObj, (ushort)40000);
        cslCfgType.GetField("segmentId").SetValue(cslObj, (ushort)42000);
        cslCfgType.GetField("manualPedestrianMode").SetValue(cslObj, false);

        Type lightDictType = cslCfgType.GetField("customLights").FieldType;
        Console.WriteLine($"customLights dictionary type: {lightDictType.FullName}");
        IDictionary lightDict = (IDictionary)Activator.CreateInstance(lightDictType);
        cslCfgType.GetField("customLights").SetValue(cslObj, lightDict);
        cslDict.Add((ushort)42000, cslObj);

        Console.WriteLine("All objects constructed successfully!");
    }
}
