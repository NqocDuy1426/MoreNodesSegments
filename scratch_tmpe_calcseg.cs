
using System;
using Mono.Cecil;
class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var t = asm.MainModule.GetType("TrafficManager.Patch._VehicleAI._CarAI.CalculateSegmentPositionPatch2");
        if (t != null) {
            foreach (var m in t.Methods) {
                Console.WriteLine(m.Name);
                if (m.HasBody) {
                    foreach (var inst in m.Body.Instructions) {
                        Console.WriteLine("  {0:X4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand);
                    }
                }
            }
        } else {
            Console.WriteLine("Type not found");
        }
    }
}
