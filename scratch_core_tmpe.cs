using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        var asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll");
        Type t1 = asm.GetType("MoreNetworkCapacity.Compatibility.TMPEIndexPatches");
        Console.WriteLine("TMPEIndexPatches: " + t1);
        foreach (var m in t1.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
        {
            Console.WriteLine("  Method: " + m.Name);
        }

        Type t2 = asm.GetType("MoreNetworkCapacity.Compatibility.TMPENetworkArrayPatches");
        Console.WriteLine("TMPENetworkArrayPatches: " + t2);
        foreach (var m in t2.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
        {
            Console.WriteLine("  Method: " + m.Name);
        }
    }
}
