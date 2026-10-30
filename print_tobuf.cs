using System;
using Mono.Cecil;
class P {
  static void Main() {
    var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
    string[] types = new string[] {
      "TrafficManager.Util.Extensions.NetNodeExtensions",
      "TrafficManager.Util.Extensions.NetLaneExtensions",
      "TrafficManager.Util.Extensions.NetSegmentExtensions"
    };
    foreach (var name in types) {
      var t = asm.MainModule.GetType(name);
      Console.WriteLine("Type: " + t.FullName);
      foreach (var f in t.Fields) Console.WriteLine("  Field: " + f.Name + " (" + f.FieldType + ")");
    }
  }
}
