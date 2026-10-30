using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll");
        var t = asm.MainModule.GetType("MoreNetworkCapacity.NetManagerCustomizer");
        var m = t.Methods.FirstOrDefault(x => x.Name == "ResetPendingUpdatesAfterLoad");
        Console.WriteLine($"=== ResetPendingUpdatesAfterLoad ===");
        if (m != null && m.HasBody)
        {
            foreach (var inst in m.Body.Instructions)
            {
                Console.WriteLine($"   {inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
            }
        }
    }
}
