using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var tmpe = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var rm = tmpe.MainModule.Types.FirstOrDefault(t => t.Name == "RoutingManager");
        foreach (var f in rm.Fields) {
            Console.WriteLine($"Field: {f.Name} : {f.FieldType.FullName}");
        }
        var ctor = rm.Methods.FirstOrDefault(m => m.Name == ".ctor");
        Console.WriteLine($"\nRoutingManager..ctor:");
        foreach (var ins in ctor.Body.Instructions) {
            Console.WriteLine($"  {ins.OpCode} {ins.Operand}");
        }
    }
}
