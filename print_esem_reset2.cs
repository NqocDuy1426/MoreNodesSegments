using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var tmpe = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var esem = tmpe.MainModule.Types.FirstOrDefault(t => t.Name == "ExtSegmentEndManager");
        foreach (var m in esem.Methods.Where(m => m.Name == "Reset")) {
            Console.WriteLine($"\n--- Reset ({string.Join(", ", m.Parameters.Select(p => p.ParameterType.Name))}) ---");
            foreach (var instr in m.Body.Instructions) {
                Console.WriteLine($"  {instr.OpCode} {instr.Operand}");
            }
        }
    }
}
