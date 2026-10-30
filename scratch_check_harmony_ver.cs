using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        var asm1 = Assembly.LoadFrom("0Harmony.dll");
        Console.WriteLine("0Harmony: " + asm1.GetName().Version);
        var asm2 = Assembly.LoadFrom("CitiesHarmony.Harmony.dll");
        Console.WriteLine("CitiesHarmony: " + asm2.GetName().Version);
        foreach (var m in asm2.GetType("Harmony.HarmonyInstance").GetMethods())
        {
            if (m.Name == "Patch")
            {
                Console.WriteLine("Patch method params:");
                foreach (var p in m.GetParameters())
                {
                    Console.WriteLine("  " + p.Name + " (" + p.ParameterType.Name + ")");
                }
            }
        }
    }
}
