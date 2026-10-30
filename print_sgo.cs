using System;
using Mono.Cecil;
class P {
  static void Main() {
    var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
    var sgo = asm.MainModule.GetType("TrafficManager.State.SavedGameOptions");
    Console.WriteLine("Type: " + sgo.FullName);
    foreach (var f in sgo.Fields) Console.WriteLine("  Field: " + f.Name + " (" + f.FieldType + ")");
    foreach (var m in sgo.Methods) {
      if (m.Name.Contains("Instance") || m.Name.Contains("Ensure") || m.Name.Contains(".ctor") || m.Name.Contains(".cctor")) {
        Console.WriteLine("\nMethod: " + m.FullName);
        if (m.HasBody) {
          foreach (var ins in m.Body.Instructions) Console.WriteLine("  " + ins);
        }
      }
    }
  }
}
