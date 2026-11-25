
using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var lm = asm.MainModule.GetType("LoadingManager");
        TypeDefinition iter = null;
        foreach (var nested in lm.NestedTypes) {
            if (nested.Name.Contains("LoadSimulationData")) iter = nested;
        }
        foreach (var m in iter.Methods) {
            if (m.Name == "MoveNext") {
                foreach (var inst in m.Body.Instructions) {
                    if (inst.Offset >= 0x01e0 && inst.Offset <= 0x0350)
                        Console.WriteLine(string.Format("  IL_{0:x4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand));
                }
            }
        }
    }
}
