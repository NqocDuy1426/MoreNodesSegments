using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1806963141/TrafficManager.dll";
        Assembly asm = Assembly.LoadFrom(tmpePath);
        Type t = asm.GetType("TrafficManager.Patch._RoadBaseAI.GetTrafficLightNodeStatePatch");
        if (t == null)
        {
            Console.WriteLine("Type not found");
            return;
        }
        foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
        {
            Console.WriteLine("Method: " + m.Name);
            var mb = m.GetMethodBody();
            if (mb != null)
            {
                byte[] il = mb.GetILAsByteArray();
                Console.WriteLine("IL bytes: " + il.Length);
            }
        }
    }
}
