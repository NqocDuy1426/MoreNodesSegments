
using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var sm = asm.MainModule.GetType("SimulationManager");
        TypeDefinition data = null;
        foreach (var nested in sm.NestedTypes) {
            if (nested.Name == "Data") data = nested;
        }
        foreach (var m in data.Methods) {
            if (m.Name == "Serialize") {
                Console.WriteLine("Method: " + m.FullName);
                foreach (var inst in m.Body.Instructions) {
                    if (inst.OpCode == OpCodes.Callvirt || inst.OpCode == OpCodes.Call) {
                        Console.WriteLine(string.Format("  IL_{0:x4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand));
                    }
                }
            }
        }
    }
}
