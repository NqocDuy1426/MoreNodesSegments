using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        Assembly asm = Assembly.LoadFrom(tmpePath);
        Console.WriteLine("Inspecting TM:PE patches on CarAI / VehicleAI:");
        foreach (Type t in asm.GetTypes())
        {
            if (t.FullName.Contains("CarAI") || t.FullName.Contains("VehicleAI") || t.FullName.Contains("VehicleBehavior"))
            {
                Console.WriteLine("Type: " + t.FullName);
                foreach (MethodInfo m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
                {
                    if (m.Name.Contains("Patch") || m.Name.Contains("Prefix") || m.Name.Contains("Postfix") || m.Name.Contains("Transpiler"))
                    {
                        Console.WriteLine("  " + m.Name);
                    }
                }
            }
        }
    }
}
