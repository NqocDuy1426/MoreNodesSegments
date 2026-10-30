using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var lm = asm.MainModule.Types.FirstOrDefault(t => t.Name == "LoadingManager");
        Console.WriteLine("LoadingManager found: " + (lm != null));
        var sm = asm.MainModule.Types.FirstOrDefault(t => t.Name == "SimulationManager");
        Console.WriteLine("SimulationManager found: " + (sm != null));
        
        // Find Managers_UpdateData and LateUpdateData
        foreach (var m in sm.Methods) {
            if (m.Name.Contains("UpdateData") || m.Name.Contains("LateUpdateData")) {
                Console.WriteLine("SM method: " + m.FullName);
                foreach (var ins in m.Body.Instructions) {
                    Console.WriteLine("  " + ins.ToString());
                }
            }
        }
    }
}
