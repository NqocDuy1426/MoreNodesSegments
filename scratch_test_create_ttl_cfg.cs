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
        Type ttlCfgType = asm.GetType("TrafficManager.Configuration+TimedTrafficLights");
        object obj = Activator.CreateInstance(ttlCfgType);
        ttlCfgType.GetField("nodeId").SetValue(obj, (ushort)40000);
        ttlCfgType.GetField("started").SetValue(obj, true);

        Console.WriteLine($"Created: nodeId={ttlCfgType.GetField("nodeId").GetValue(obj)}, started={ttlCfgType.GetField("started").GetValue(obj)}");
    }
}
