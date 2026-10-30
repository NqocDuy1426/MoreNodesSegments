using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var tmpe = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var esm = tmpe.MainModule.Types.FirstOrDefault(t => t.Name == "ExtSegmentManager");
        var m = esm.Methods.FirstOrDefault(x => x.Name == "OnBeforeLoadData");
        Console.WriteLine($"Method: {m.Name}");
        foreach (var ins in m.Body.Instructions) {
            Console.WriteLine($"  {ins.OpCode} {ins.Operand}");
        }
    }
}
