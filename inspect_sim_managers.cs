using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var sm = asm.MainModule.Types.FirstOrDefault(t => t.Name == "SimulationManager");
        var m = sm.Methods.FirstOrDefault(x => x.Name == "Awake");
        Console.WriteLine("SimulationManager Awake: " + (m != null ? m.FullName : "null"));
        if (m != null) {
            foreach (var ins in m.Body.Instructions) {
                if (ins.OpCode == OpCodes.Callvirt && ins.Operand.ToString().Contains("Add")) {
                    Console.WriteLine("  " + ins.ToString());
                }
            }
        }
        // Let's find all types implementing ISimulationManager
        var iface = asm.MainModule.Types.FirstOrDefault(t => t.Name == "ISimulationManager");
        foreach (var t in asm.MainModule.Types) {
            if (t.Interfaces.Any(i => i.InterfaceType.Name == "ISimulationManager")) {
                Console.WriteLine("Implements ISimulationManager: " + t.FullName);
            }
        }
    }
}
