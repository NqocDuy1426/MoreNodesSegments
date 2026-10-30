using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var tmpe = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var rm = tmpe.MainModule.Types.FirstOrDefault(t => t.Name == "RoutingManager");
        var m1 = rm.Methods.FirstOrDefault(m => m.Name == "RecalculateLaneEndRoutingData");
        Console.WriteLine($"\n--- {m1.Name} ---");
        foreach (var ins in m1.Body.Instructions.Take(50)) {
            Console.WriteLine($"  {ins.Offset:X4} {ins.OpCode} {ins.Operand}");
        }
    }
}
