using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll");
        var t = asm.MainModule.GetType("MoreNetworkCapacity.Patches.NetManagerDeserializePatch");
        var m = t.Methods.FirstOrDefault(x => x.Name == "Prefix");
        Console.WriteLine($"=== NetManagerDeserializePatch.Prefix instructions: {m.Body.Instructions.Count} ===");
        foreach (var inst in m.Body.Instructions)
        {
            Console.WriteLine($"   {inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
        }
    }
}
