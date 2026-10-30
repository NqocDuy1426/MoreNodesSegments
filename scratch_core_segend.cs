using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        var asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll");
        Type t = asm.GetType("MoreNetworkCapacity.Compatibility.TMPEIndexPatches");
        MethodInfo m = t.GetMethod("SegmentEndGetIndexPrefix", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        Console.WriteLine("SegmentEndGetIndexPrefix: " + m);
        foreach (var p in m.GetParameters()) Console.WriteLine("  " + p.ParameterType + " " + p.Name);
        byte[] il = m.GetMethodBody().GetILAsByteArray();
        for (int i = 0; i < il.Length; i++)
        {
            Console.WriteLine(string.Format("{0:X2}: {1:X2}", i, il[i]));
        }
    }
}
