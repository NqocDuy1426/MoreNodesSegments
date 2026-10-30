using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string dir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/";
        Assembly asm = Assembly.LoadFrom(dir + "TrafficManager.dll");
        Type t = asm.GetType("TrafficManager.Manager.Impl.TrafficLightSimulationManager");
        MethodInfo m = t.GetMethod("SaveData", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        var body = m.GetMethodBody();
        byte[] il = body.GetILAsByteArray();

        // Print around IL_02E0 to IL_0310
        for (int i = 0x02E0; i < 0x0310; i++)
        {
            Console.Write($"{il[i]:X2} ");
        }
        Console.WriteLine();
    }
}
