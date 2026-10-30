using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("content/MoreNetworkCapacity.Core.dll");
        var t = asm.MainModule.Types.First(x => x.Name == "NetManagerCustomizer");
        foreach (var m in t.Methods) {
            if (m.Name.Contains("Prepare") || m.Name.Contains("IsExpanded") || m.Name.Contains("Apply")) {
                Console.WriteLine($"=== Method: {m.FullName} ===");
                if (m.HasBody) {
                    foreach (var instr in m.Body.Instructions) {
                        Console.WriteLine($"{instr.Offset:X4}: {instr.OpCode} {instr.Operand}");
                    }
                }
            }
        }
    }
}
