using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var tmpe = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var esm = tmpe.MainModule.Types.FirstOrDefault(t => t.Name == "ExtSegmentManager");
        var ctor = esm.Methods.FirstOrDefault(m => m.Name == ".ctor");
        Console.WriteLine($"ExtSegmentManager..ctor:");
        foreach (var ins in ctor.Body.Instructions) {
            Console.WriteLine($"  {ins.OpCode} {ins.Operand}");
        }

        TypeDefinition extSeg = tmpe.MainModule.Types.FirstOrDefault(t => t.Name == "ExtSegment");
        if (extSeg == null) {
            var api = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TMPE.API.dll");
            extSeg = api.MainModule.Types.FirstOrDefault(t => t.Name == "ExtSegment");
        }
        Console.WriteLine($"ExtSegment isValueType: {extSeg.IsValueType}");
        foreach (var f in extSeg.Fields) {
            Console.WriteLine($"  Field: {f.Name} : {f.FieldType.FullName}");
        }
        foreach (var m in extSeg.Methods.Where(m => m.Name == ".ctor")) {
            Console.WriteLine($"  Method: {m.Name}");
            foreach (var ins in m.Body.Instructions) {
                Console.WriteLine($"    {ins.OpCode} {ins.Operand}");
            }
        }
    }
}
