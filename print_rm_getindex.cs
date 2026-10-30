using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var tmpe = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var rm = tmpe.MainModule.Types.FirstOrDefault(t => t.Name == "RoutingManager");
        var m = rm.Methods.FirstOrDefault(x => x.Name.Contains("Index") || x.Name.Contains("LaneEndRouting"));
        if (m != null) {
            Console.WriteLine($"Method: {m.Name}");
            foreach (var ins in m.Body.Instructions) {
                Console.WriteLine($"  {ins.OpCode} {ins.Operand}");
            }
        }
    }
}
