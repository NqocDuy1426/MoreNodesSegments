using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var tmpe = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var rm = tmpe.MainModule.Types.FirstOrDefault(t => t.Name == "RoutingManager");
        foreach (var m in rm.Methods) {
            Console.WriteLine($"Method: {m.Name} ({string.Join(", ", m.Parameters.Select(p => p.ParameterType.Name))})");
        }
    }
}
