using System;
using Mono.Cecil;
class P {
  static void Main() {
    var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
    var t = asm.MainModule.GetType("TrafficManager.Util.Extensions.NetSegmentExtensions");
    Console.WriteLine("Type: " + t.FullName);
    foreach (var f in t.Fields) Console.WriteLine("  Field: " + f.Name + " (" + f.FieldType + ")");
    foreach (var m in t.Methods) {
      if (m.Name == "ToSegment") {
        Console.WriteLine("\nMethod: " + m.FullName);
        foreach (var ins in m.Body.Instructions) Console.WriteLine("  " + ins);
      }
    }
  }
}
