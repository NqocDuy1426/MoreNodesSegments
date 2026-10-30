using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        foreach (var type in asm.MainModule.Types) {
            CheckType(type);
        }
    }
    static void CheckType(TypeDefinition t) {
        foreach (var m in t.Methods) {
            if (m.HasBody) {
                foreach (var ins in m.Body.Instructions) {
                    if (ins.OpCode == OpCodes.Call || ins.OpCode == OpCodes.Callvirt) {
                        var mr = ins.Operand as MethodReference;
                        if (mr != null && mr.Name.Contains("InitializeProperties")) {
                            Console.WriteLine($"{t.FullName}::{m.Name} calls {mr.FullName}");
                        }
                    }
                }
            }
        }
        foreach (var nested in t.NestedTypes) {
            CheckType(nested);
        }
    }
}
