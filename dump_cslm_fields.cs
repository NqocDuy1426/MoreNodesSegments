
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var t = asm.MainModule.GetType("TrafficManager.Manager.Impl.CustomSegmentLightsManager");
        Console.WriteLine("Fields in CustomSegmentLightsManager:");
        foreach (var f in t.Fields) {
            Console.WriteLine($"  {f.Name} : {f.FieldType.FullName}");
        }
    }
}
