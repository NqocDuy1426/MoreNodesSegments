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
        Console.WriteLine($"=== NetManagerCustomizer methods ===");
        foreach (var m in t.Methods)
        {
            Console.WriteLine($"Method: {m.Name} ({string.Join(", ", m.Parameters.Select(p => p.ParameterType.Name))})");
            foreach (var inst in m.Body.Instructions)
            {
                if (inst.OpCode == OpCodes.Throw || (inst.OpCode == OpCodes.Ldstr && inst.Operand.ToString().Contains("refusing")))
                {
                    Console.WriteLine($"   {inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                }
            }
        }
    }
}
