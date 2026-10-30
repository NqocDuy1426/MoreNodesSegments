using System;
using System.Reflection;

public class PrintGeneratedTargets
{
    public static void Main()
    {
        var asm = Assembly.LoadFrom("MoreNetworkCapacity.Core.dll");
        var t = asm.GetType("MoreNetworkCapacity.Patches.GeneratedPatchTargets");
        var fld = t.GetField("BranchBoundMethods", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
        var arr = (string[][])fld.GetValue(null);
        Console.WriteLine("BranchBoundMethods count: " + arr.Length);
        bool hasTL = false;
        foreach (var sub in arr)
        {
            if (sub[0].Contains("Transport"))
            {
                Console.WriteLine("  " + sub[0] + "::" + sub[1]);
                hasTL = true;
            }
        }
        if (!hasTL) Console.WriteLine("  NO TRANSPORT METHODS FOUND!");
    }
}
