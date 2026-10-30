using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var tls = asm.MainModule.GetType("TrafficManager.TrafficLight.Impl.TrafficLightSimulation");
        foreach (var m in tls.Methods) {
            if (m.Name == "SimulationStep") {
                Console.WriteLine("Method: " + m.FullName);
                foreach (var inst in m.Body.Instructions) {
                    Console.WriteLine(inst);
                }
            }
        }
    }
}
