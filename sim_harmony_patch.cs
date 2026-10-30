using System;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using Mono.Cecil;

class SimPatch
{
    static void Main()
    {
        var asm = Assembly.LoadFrom("content/MoreNetworkCapacity.Core.dll");
        var patchType = asm.GetType("MoreNetworkCapacity.Patches.NetManagerDeserializePatch");
        var transpilerMethod = patchType.GetMethod("Transpiler", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        Console.WriteLine("Found transpilerMethod: " + transpilerMethod);
    }
}
