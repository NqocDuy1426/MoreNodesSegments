using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var tmpe = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var nne = tmpe.MainModule.Types.FirstOrDefault(t => t.Name == "NetNodeExtensions");
        var m = nne.Methods.FirstOrDefault(x => x.Name == "ToNode");
        Console.WriteLine($"Method: {m.Name}");
        foreach (var ins in m.Body.Instructions) {
            Console.WriteLine($"  {ins.OpCode} {ins.Operand}");
        }
    }
}
