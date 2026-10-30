
using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var t = asm.MainModule.GetType("TrafficManager.Patch._VehicleAI.VehicleAICommons");
        foreach (var m in t.Methods) {
            if (m.Name.Contains("CustomCalculateTargetSpeed")) {
                Console.WriteLine("Method: " + m.FullName);
                foreach (var ins in m.Body.Instructions) {
                    if (ins.OpCode == OpCodes.Call || ins.OpCode == OpCodes.Callvirt) {
                        Console.WriteLine("  calls " + ins.Operand);
                    }
                }
            }
        }
    }
}
