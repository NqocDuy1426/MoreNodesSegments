using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        foreach (var t in asm.MainModule.Types) {
            CheckType(t);
        }
    }
    static void CheckType(TypeDefinition t) {
        foreach (var m in t.Methods) {
            if (m.HasBody) {
                foreach (var ins in m.Body.Instructions) {
                    if (ins.OpCode.Code == Mono.Cecil.Cil.Code.Ldc_I4) {
                        int val = (int)ins.Operand;
                        if (val == 32768 || val == 36864 || val == 73728) {
                            Console.WriteLine($"{t.FullName}::{m.Name} uses {val}");
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
