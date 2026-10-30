using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        Assembly asm = Assembly.LoadFrom(tmpePath);
        Type rmT = asm.GetType("TrafficManager.Manager.Impl.RoutingManager");
        MethodInfo m = rmT.GetMethod("RequestFullRecalculation", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        Console.WriteLine("Method: " + m);
        byte[] il = m.GetMethodBody().GetILAsByteArray();
        Console.WriteLine("IL length: " + il.Length);
        for (int i = 0; i < il.Length; i++)
        {
            Console.WriteLine(string.Format("{0:X2}: {1:X2}", i, il[i]));
        }
    }
}
