using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var patch = asm.MainModule.GetType("TrafficManager.Patch._VehicleAI._CarAI.CalculateSegmentPositionPatch");
        if (patch != null) {
            foreach (var m in patch.Methods) {
                Console.WriteLine("Method: " + m.FullName);
                if (m.HasBody) {
                    foreach (var inst in m.Body.Instructions) {
                        Console.WriteLine("  " + inst);
                    }
                }
            }
        }
    }
}
