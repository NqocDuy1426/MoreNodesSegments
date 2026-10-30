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
                    if (ins.OpCode == OpCodes.Ldstr && ins.Operand != null) {
                        string s = ins.Operand.ToString();
                        if (s.IndexOf("Daynight", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            s.IndexOf("Weather Properties", StringComparison.OrdinalIgnoreCase) >= 0) {
                            Console.WriteLine($"{t.FullName}::{m.Name} -> \"{s}\"");
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
