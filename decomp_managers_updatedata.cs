using System;
using System.Reflection;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var t = asm.MainModule.GetType("SimulationManager");
        foreach (var m in t.Methods) {
            if (m.Name.Contains("UpdateData") || m.Name.Contains("Managers_UpdateData")) {
                Console.WriteLine("=== Method: " + m.FullName + " ===");
                if (m.HasBody) {
                    foreach (var i in m.Body.Instructions) {
                        Console.WriteLine("  " + i);
                    }
                }
            }
        }
    }
}
