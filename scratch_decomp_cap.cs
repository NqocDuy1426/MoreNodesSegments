using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("content/MoreNetworkCapacity.Core.dll");
        var t = asm.MainModule.Types.First(x => x.Name == "Capacity");
        foreach (var m in t.Methods) {
            Console.WriteLine($"=== Method: {m.FullName} ===");
            if (m.HasBody) {
                foreach (var instr in m.Body.Instructions) {
                    Console.WriteLine($"{instr.Offset:X4}: {instr.OpCode} {instr.Operand}");
                }
            }
        }
    }
}
