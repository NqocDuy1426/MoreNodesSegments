using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var tmpe = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var type = tmpe.MainModule.Types.FirstOrDefault(t => t.Name == "CustomSegmentLights");
        var m = type.Methods.FirstOrDefault(x => x.Name == "CalculateAutoPedestrianLightState");
        Console.WriteLine($"Method: {m.Name}");
        foreach (var ins in m.Body.Instructions) {
            Console.WriteLine($"  {ins.Offset:X4} {ins.OpCode} {ins.Operand}");
        }
    }
}
