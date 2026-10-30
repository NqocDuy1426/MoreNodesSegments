using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var tmpe = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var esem = tmpe.MainModule.Types.FirstOrDefault(t => t.Name == "ExtSegmentEndManager");
        var reset = esem.Methods.FirstOrDefault(m => m.Name == "Reset");
        Console.WriteLine($"Method: {reset.Name}");
        foreach (var instr in reset.Body.Instructions) {
            Console.WriteLine($"  {instr.OpCode} {instr.Operand}");
        }
    }
}
