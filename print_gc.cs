using System;
using Mono.Cecil;
class P {
  static void Main() {
    var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
    var gc = asm.MainModule.GetType("TrafficManager.State.GlobalConfig");
    Console.WriteLine("Type: " + gc.FullName);
    foreach (var m in gc.Methods) {
      if (m.Name.Contains("Instance") || m.Name.Contains("Write") || m.Name.Contains("Reload") || m.Name.Contains(".cctor")) {
        Console.WriteLine("\nMethod: " + m.FullName);
      }
    }
  }
}
