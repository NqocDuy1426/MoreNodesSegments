using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var tmpe = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var esem = tmpe.MainModule.Types.FirstOrDefault(t => t.Name == "ExtSegmentEndManager");
        var m = esem.Methods.FirstOrDefault(x => x.Name == "OnLevelLoading");
        Console.WriteLine($"Method: {m.Name}");
        foreach (var ins in m.Body.Instructions) {
            Console.WriteLine($"  {ins.OpCode} {ins.Operand}");
        }
    }
}
