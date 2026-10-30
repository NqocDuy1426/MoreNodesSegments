using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        Assembly asm = Assembly.LoadFrom(tmpePath);
        Type t = asm.GetType("TrafficManager.Manager.Impl.ExtVehicleManager");
        MethodInfo m = t.GetMethod("InitAllVehicles", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        byte[] il = m.GetMethodBody().GetILAsByteArray();
        for (int i = 0; i < il.Length; i++)
        {
            Console.WriteLine(string.Format("{0:X2}: {1:X2}", i, il[i]));
        }
    }
}
