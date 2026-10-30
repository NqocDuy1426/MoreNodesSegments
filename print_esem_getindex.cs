using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var tmpe = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var esem = tmpe.MainModule.Types.FirstOrDefault(t => t.Name == "ExtSegmentEndManager");
        var gi = esem.Methods.FirstOrDefault(m => m.Name == "GetIndex");
        Console.WriteLine($"Method: {gi.Name}");
        foreach (var ins in gi.Body.Instructions) {
            Console.WriteLine($"  {ins.OpCode} {ins.Operand}");
        }
    }
}
