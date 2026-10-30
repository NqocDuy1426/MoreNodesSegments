using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        var asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll");
        Type t = asm.GetType("MoreNetworkCapacity.Patches.NodeSimulationPhasePatch");
        MethodInfo m = t.GetMethod("FrameSource", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        Console.WriteLine("FrameSource: " + m);
        MethodInfo m2 = t.GetMethod("AddressIndex", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        Console.WriteLine("AddressIndex: " + m2);
        MethodInfo m3 = t.GetMethod("Argument", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        Console.WriteLine("Argument: " + m3);
    }
}
