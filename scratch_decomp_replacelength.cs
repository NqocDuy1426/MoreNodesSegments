using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("content/MoreNetworkCapacity.Core.dll");
        var t = asm.MainModule.Types.First(x => x.Name == "NetworkBufferSizePatch");
        var m = t.Methods.First(x => x.Name == "ReplaceLength");
        Console.WriteLine($"=== Method: {m.FullName} ===");
        foreach (var instr in m.Body.Instructions) {
            Console.WriteLine($"{instr.Offset:X4}: {instr.OpCode} {instr.Operand}");
        }
    }
}
