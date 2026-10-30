
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var t = asm.MainModule.GetType("TrafficManager.Manager.Impl.TrafficLightSimulationManager");
        Console.WriteLine("Fields in TrafficLightSimulationManager:");
        foreach (var f in t.Fields) {
            Console.WriteLine($"  {f.Name} : {f.FieldType.FullName}");
        }
        Console.WriteLine("Properties in TrafficLightSimulationManager:");
        foreach (var p in t.Properties) {
            Console.WriteLine($"  {p.Name} : {p.PropertyType.FullName} (get: {p.GetMethod?.Name}, set: {p.SetMethod?.Name})");
        }
    }
}
