using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var tmpe = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        TypeDefinition extEnd = tmpe.MainModule.Types.FirstOrDefault(t => t.Name == "ExtSegmentEnd");
        if (extEnd == null) {
            var api = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TMPE.API.dll");
            extEnd = api.MainModule.Types.FirstOrDefault(t => t.Name == "ExtSegmentEnd");
        }
        Console.WriteLine($"ExtSegmentEnd isValueType: {extEnd.IsValueType}");
        foreach (var f in extEnd.Fields) {
            Console.WriteLine($"  Field: {f.Name} : {f.FieldType.FullName}");
        }
        foreach (var m in extEnd.Methods) {
            Console.WriteLine($"  Method: {m.Name} ({string.Join(", ", m.Parameters.Select(p => p.ParameterType.Name))})");
            if (m.Name == ".ctor") {
                foreach (var ins in m.Body.Instructions) {
                    Console.WriteLine($"    {ins.OpCode} {ins.Operand}");
                }
            }
        }
    }
}
