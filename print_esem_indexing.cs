using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var tmpe = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var esem = tmpe.MainModule.Types.FirstOrDefault(t => t.Name == "ExtSegmentEndManager");
        Console.WriteLine("ExtSegmentEndManager methods:");
        foreach (var m in esem.Methods) {
            if (m.Name.Contains("Index") || m.Name.Contains("End") || m.Name.Contains("Recalculate")) {
                Console.WriteLine($"\n--- Method: {m.Name} ({string.Join(", ", m.Parameters.Select(p => p.ParameterType.Name))}) ---");
                foreach (var instr in m.Body.Instructions) {
                    Console.WriteLine($"  {instr.OpCode} {instr.Operand}");
                }
            }
        }
        Console.WriteLine("\nExtSegmentEndManager Fields:");
        foreach (var f in esem.Fields) {
            Console.WriteLine($"Field: {f.Name} : {f.FieldType.FullName}");
        }
    }
}
