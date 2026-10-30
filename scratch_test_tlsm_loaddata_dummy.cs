using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        string dir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/";
        Assembly asm = Assembly.LoadFrom(dir + "TrafficManager.dll");
        Type t = asm.GetType("TrafficManager.Manager.Impl.TrafficLightSimulationManager");
        Type ttlType = asm.GetType("TrafficManager.Configuration+TimedTrafficLights");
        Type listType = typeof(List<>).MakeGenericType(ttlType);

        MethodInfo mLoad = t.GetMethod("LoadData", new Type[] { listType });
        Console.WriteLine($"Method found: {mLoad.Name}");
    }
}
