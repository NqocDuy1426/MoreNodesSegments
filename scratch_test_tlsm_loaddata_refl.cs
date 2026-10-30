using System;
using System.Reflection;
using System.Collections;

class Program
{
    static void Main()
    {
        string dir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/";
        Assembly asm = Assembly.LoadFrom(dir + "TrafficManager.dll");
        Type tlsmType = asm.GetType("TrafficManager.Manager.Impl.TrafficLightSimulationManager");
        Type listType = typeof(System.Collections.Generic.List<>).MakeGenericType(asm.GetType("TrafficManager.Configuration+TimedTrafficLights"));

        MethodInfo m = tlsmType.GetMethod("LoadData", new Type[] { listType });
        Console.WriteLine($"Found LoadData: {m != null}");
    }
}
