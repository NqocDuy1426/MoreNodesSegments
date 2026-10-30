using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string dir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/";
        Assembly asm = Assembly.LoadFrom(dir + "TrafficManager.dll");
        Type tpmType = asm.GetType("TrafficManager.Manager.Impl.TrafficPriorityManager");
        Type ptType = Assembly.LoadFrom(dir + "TMPE.API.dll").GetType("TrafficManager.API.Traffic.Enums.PriorityType");

        MethodInfo mSet = tpmType.GetMethod("SetPrioritySign", new Type[] { typeof(ushort), typeof(bool), ptType });
        MethodInfo mGet = tpmType.GetMethod("GetPrioritySign", new Type[] { typeof(ushort), typeof(bool) });

        Console.WriteLine($"mSet: {mSet != null}, mGet: {mGet != null}");
    }
}
