using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var sm = asm.MainModule.Types.FirstOrDefault(t => t.Name == "SimulationManager");
        foreach (var m in sm.Methods) {
            if (m.Name.StartsWith("Managers_")) {
                Console.WriteLine($"{m.Name}: IsStatic={m.IsStatic}, IsPublic={m.IsPublic}");
            }
        }
    }
}
