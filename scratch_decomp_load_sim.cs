using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var lm = asm.MainModule.Types.First(t => t.Name == "LoadingManager");
        var iter = lm.NestedTypes.FirstOrDefault(t => t.Name.Contains("LoadSimulationData") && t.Name.Contains("Iterator"));
        if (iter != null) {
            var moveNext = iter.Methods.FirstOrDefault(m => m.Name == "MoveNext");
            if (moveNext != null) {
                foreach (var instr in moveNext.Body.Instructions) {
                    Console.WriteLine($"{instr.Offset:X4}: {instr.OpCode} {instr.Operand}");
                }
            }
        }
    }
}
