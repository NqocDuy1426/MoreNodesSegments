using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var tmpe = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var enm = tmpe.MainModule.Types.FirstOrDefault(t => t.Name == "ExtNodeManager");
        Console.WriteLine($"ExtNodeManager methods and fields:");
        foreach (var f in enm.Fields) {
            Console.WriteLine($"  Field: {f.Name} : {f.FieldType.FullName}");
        }
        var ctor = enm.Methods.FirstOrDefault(m => m.Name == ".ctor");
        if (ctor != null) {
            Console.WriteLine($"ExtNodeManager..ctor:");
            foreach (var ins in ctor.Body.Instructions) {
                Console.WriteLine($"    {ins.OpCode} {ins.Operand}");
            }
        }
    }
}
