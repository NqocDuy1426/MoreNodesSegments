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

        Type tlsmType = tmpe.GetType("TrafficManager.Manager.Impl.TrafficLightSimulationManager");
        Type ttlCfgType = tmpe.GetType("TrafficManager.Configuration+TimedTrafficLights");
        Type listType = typeof(List<>).MakeGenericType(ttlCfgType);
        MethodInfo mLoad = tlsmType.GetMethod("LoadData", new Type[] { listType });

        Console.WriteLine("mLoad method: " + (mLoad != null));
    }
}
