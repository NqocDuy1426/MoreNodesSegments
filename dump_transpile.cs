
using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var t = asm.MainModule.GetType("TrafficManager.Patch._VehicleAI.VehicleAICommons");
        foreach (var m in t.Methods) {
            if (m.Name == "TranspileCalculateSegmentPosition") {
                Console.WriteLine("Method: " + m.FullName);
                foreach (var ins in m.Body.Instructions) {
                    Console.WriteLine("  " + ins);
                }
            }
        }
    }
}
