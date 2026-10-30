using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        var asm = Assembly.LoadFrom("CitiesHarmony.Harmony.dll");
        var hType = asm.GetType("HarmonyLib.Harmony");
        Console.WriteLine("HarmonyLib.Harmony type: " + (hType != null));
        foreach (var m in hType.GetMethods())
        {
            if (m.Name == "Patch")
            {
                Console.WriteLine("Patch params:");
                foreach (var p in m.GetParameters())
                {
                    Console.WriteLine("  " + p.Name + " (" + p.ParameterType.Name + ")");
                }
            }
        }
    }
}
