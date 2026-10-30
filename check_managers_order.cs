using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var t = asm.MainModule.GetType("SimulationManager");
        var m = t.Methods;
        foreach (var method in m) {
            if (method.Name == "Awake") {
                foreach (var i in method.Body.Instructions) {
                    if (i.Operand != null && i.Operand.ToString().Contains("RegisterManager")) {
                        Console.WriteLine("  " + i.Previous + " -> " + i);
                    }
                }
            }
        }
    }
}
