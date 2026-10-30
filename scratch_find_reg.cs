using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        foreach (var t in asm.MainModule.Types) {
            foreach (var m in t.Methods) {
                if (m.Body != null) {
                    foreach (var i in m.Body.Instructions) {
                        if (i.OpCode == OpCodes.Call && i.Operand != null && i.Operand.ToString().Contains("RegisterSimulationManager")) {
                            Console.WriteLine(t.FullName + "::" + m.Name + " -> " + i.Operand);
                        }
                    }
                }
            }
        }
    }
}

