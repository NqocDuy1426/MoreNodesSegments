using System;
using Mono.Cecil;
class P {
  static void Main() {
    var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
    var sgo = asm.MainModule.GetType("TrafficManager.State.SavedGameOptions");
    foreach (var m in sgo.Methods) {
      if (m.Name == "Create") {
        Console.WriteLine("\nMethod: " + m.FullName);
        foreach (var ins in m.Body.Instructions) Console.WriteLine("  " + ins);
      }
    }
  }
}
