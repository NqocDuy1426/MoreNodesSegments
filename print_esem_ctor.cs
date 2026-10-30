using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var tmpe = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var esem = tmpe.MainModule.Types.FirstOrDefault(t => t.Name == "ExtSegmentEndManager");
        var ctor = esem.Methods.FirstOrDefault(m => m.Name == ".ctor");
        Console.WriteLine($"Method: {ctor.Name}");
        foreach (var instr in ctor.Body.Instructions) {
            Console.WriteLine($"  {instr.OpCode} {instr.Operand}");
        }
    }
}
