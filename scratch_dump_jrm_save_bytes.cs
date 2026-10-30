using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        Assembly asm = Assembly.LoadFrom(tmpePath);
        Type t = asm.GetType("TrafficManager.Manager.Impl.JunctionRestrictionsManager");
        MethodInfo m = t.GetMethod("SaveData", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        var body = m.GetMethodBody();
        byte[] il = body.GetILAsByteArray();

        for (int i = 0; i < 50; i++)
        {
            Console.Write($"{il[i]:X2} ");
        }
        Console.WriteLine();
    }
}
