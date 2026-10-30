using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var patch = asm.MainModule.GetType("TrafficManager.Patch._VehicleAI._CarAI.CalculateSegmentPositionPatch2");
        foreach (var m in patch.Methods) {
            if (m.Name == "Prefix") {
                Console.WriteLine("Prefix parameters:");
                for (int i = 0; i < m.Parameters.Count; i++) {
                    Console.WriteLine(string.Format("  [{0}] {1} {2}", i, m.Parameters[i].ParameterType.Name, m.Parameters[i].Name));
                }
            }
        }
    }
}
