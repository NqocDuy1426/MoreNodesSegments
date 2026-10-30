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

        Console.WriteLine("Config types: " + (ttlCfgType != null && stepCfgType != null && cslCfgType != null && lightCfgType != null));

        object ttlObj = Activator.CreateInstance(ttlCfgType);
        object stepObj = Activator.CreateInstance(stepCfgType);
        object cslObj = Activator.CreateInstance(cslCfgType);
        object lightObj = Activator.CreateInstance(lightCfgType);

        Console.WriteLine("Instantiation success!");
    }
}
