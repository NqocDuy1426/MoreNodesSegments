using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        var asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll");
        Type t = asm.GetType("MoreNetworkCapacity.Patches.NodeSimulationPhasePatch");
        Console.WriteLine("Type: " + t);
        foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
        {
            Console.WriteLine("Method: " + m.Name);
        }
    }
}
