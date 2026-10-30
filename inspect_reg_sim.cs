using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var sm = asm.MainModule.Types.FirstOrDefault(t => t.Name == "SimulationManager");
        var m = sm.Methods.FirstOrDefault(x => x.Name == "RegisterSimulationManager");
        Console.WriteLine("RegisterSimulationManager: " + (m != null ? m.FullName : "null"));
        if (m != null) {
            foreach (var ins in m.Body.Instructions) {
                Console.WriteLine("  " + ins.ToString());
            }
        }
    }
}
