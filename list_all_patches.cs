using System;
using System.Reflection;
using Harmony;

public class ListAllPatches
{
    public static void Main()
    {
        var asm = Assembly.LoadFrom("MoreNetworkCapacityRenew.dll");
        Console.WriteLine("Types in MoreNetworkCapacityRenew.dll: " + asm.GetTypes().Length);
        foreach (var t in asm.GetTypes())
        {
            var attrs = t.GetCustomAttributes(typeof(HarmonyPatch), false);
            if (attrs.Length > 0)
            {
                Console.WriteLine("HarmonyPatch: " + t.FullName);
            }
        }
    }
}
