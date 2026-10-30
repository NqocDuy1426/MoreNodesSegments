using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        foreach (var t in asm.MainModule.Types) {
            CheckType(t);
        }
    }
    static void CheckType(TypeDefinition t) {
        foreach (var m in t.Methods) {
            if (m.HasBody) {
                foreach (var ins in m.Body.Instructions) {
                    if (ins.Operand != null && ins.Operand.ToString().Contains("m_managers")) {
                        Console.WriteLine($"{t.FullName}::{m.Name} -> {ins}");
                    }
                }
            }
        }
        foreach (var nt in t.NestedTypes) {
            CheckType(nt);
        }
    }
}
