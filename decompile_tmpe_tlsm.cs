using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var tlsm = asm.MainModule.GetType("TrafficManager.Manager.Impl.TrafficLightSimulationManager");
        foreach (var m in tlsm.Methods) {
            if (m.Name == "SimulationStep") {
                Console.WriteLine("Method: " + m.FullName);
                foreach (var inst in m.Body.Instructions) {
                    Console.WriteLine(inst);
                }
            }
        }
    }
}
