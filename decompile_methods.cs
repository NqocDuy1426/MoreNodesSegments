using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        string tmDll = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        Assembly asm = Assembly.LoadFrom(tmDll);

        PrintMethod(asm.GetType("TrafficManager.TrafficLight.Impl.TimedTrafficLights"), "Stop");
        PrintMethod(asm.GetType("TrafficManager.TrafficLight.Impl.TimedTrafficLights"), "Housekeeping");
        PrintMethod(asm.GetType("TrafficManager.Manager.Impl.TrafficLightSimulationManager"), "LoadData");
        PrintMethod(asm.GetType("TrafficManager.UI.SubTools.TTL.TimedTrafficLightsTool"), "OnPrimaryClickOverlay");
    }

    static void PrintMethod(Type t, string methodName)
    {
        Console.WriteLine("\n================ " + t.FullName + "." + methodName + " ================");
        var m = t.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        if (m == null) { Console.WriteLine("Method not found!"); return; }
        MethodBody body = m.GetMethodBody();
        if (body == null) { Console.WriteLine("No body"); return; }
        byte[] il = body.GetILAsByteArray();
        Console.WriteLine("IL length: " + il.Length + " bytes, Local variables: " + body.LocalVariables.Count);
        foreach (var loc in body.LocalVariables)
        {
            Console.WriteLine("  Local " + loc.LocalIndex + ": " + loc.LocalType);
        }
    }
}
