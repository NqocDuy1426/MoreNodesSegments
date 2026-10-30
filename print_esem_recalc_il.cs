using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var tmpe = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var esem = tmpe.MainModule.Types.FirstOrDefault(t => t.Name == "ExtSegmentEndManager");
        var m = esem.Methods.FirstOrDefault(x => x.Name == "Recalculate" && x.Parameters.Count == 1 && x.Parameters[0].ParameterType.Name.Contains("&"));
        Console.WriteLine($"Method: {m.Name}");
        foreach (var ins in m.Body.Instructions) {
            Console.WriteLine($"  {ins.Offset:X4} {ins.OpCode} {ins.Operand}");
        }
    }
}
