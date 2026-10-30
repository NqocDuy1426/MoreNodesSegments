using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll");
        var t = asm.MainModule.GetType("MoreNetworkCapacity.PatchState");
        Console.WriteLine($"=== PatchState ===");
        foreach (var m in t.Methods)
        {
            Console.WriteLine($"Method: {m.Name}");
            if (m.HasBody)
            {
                foreach (var inst in m.Body.Instructions)
                {
                    Console.WriteLine($"   {inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                }
            }
        }
    }
}
