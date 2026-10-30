using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var lm = asm.MainModule.Types.FirstOrDefault(t => t.Name == "LoadingManager");
        var m = lm.Methods.FirstOrDefault(x => x.Name == "SetSimulationProgress");
        Console.WriteLine("SetSimulationProgress: " + (m != null ? m.FullName : "null"));
        if (m != null) {
            foreach (var ins in m.Body.Instructions) {
                Console.WriteLine("  " + ins.ToString());
            }
        }
    }
}
