using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var tmpe = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var esm = tmpe.MainModule.Types.FirstOrDefault(t => t.Name == "ExtSegmentManager");
        foreach (var m in esm.Methods.Where(m => m.Name == "Recalculate")) {
            Console.WriteLine($"\n--- ExtSegmentManager.Recalculate ({string.Join(", ", m.Parameters.Select(p => p.ParameterType.Name))}) ---");
            foreach (var instr in m.Body.Instructions) {
                Console.WriteLine($"  {instr.OpCode} {instr.Operand}");
            }
        }
    }
}
