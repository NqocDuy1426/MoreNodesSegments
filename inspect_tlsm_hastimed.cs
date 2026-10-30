using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var t = asm.MainModule.GetType("TrafficManager.Manager.Impl.TrafficLightSimulationManager");
        foreach (var m in t.Methods) {
            if (m.Name == "HasTimedSimulation" || m.Name == "HasSimulation") {
                Console.WriteLine("=== " + m.FullName + " ===");
                foreach (var ins in m.Body.Instructions) {
                    Console.WriteLine("  " + ins);
                }
            }
        }
    }
}
