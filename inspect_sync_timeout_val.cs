using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var sm = asm.MainModule.Types.FirstOrDefault(t => t.Name == "SimulationManager");
        var m = sm.Methods.FirstOrDefault(x => x.Name == ".cctor");
        for (int i = 0; i < 5; i++) {
            Console.WriteLine("  " + m.Body.Instructions[i].ToString());
        }
    }
}
