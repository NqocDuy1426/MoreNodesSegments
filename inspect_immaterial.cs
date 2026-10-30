using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        foreach (var t in asm.MainModule.Types) {
            foreach (var m in t.Methods) {
                if (m.HasBody) {
                    foreach (var ins in m.Body.Instructions) {
                        if (ins.OpCode == OpCodes.Ldstr && ins.Operand.ToString().Contains("ImmaterialResourceManager")) {
                            Console.WriteLine($"{t.FullName}::{m.Name} -> {ins.Operand}");
                        }
                    }
                }
            }
        }
    }
}
