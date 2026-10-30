using System;
using Mono.Cecil;
class P {
  static void Main() {
    var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
    var sde = asm.MainModule.GetType("TrafficManager.Lifecycle.SerializableDataExtension");
    var m = sde.Methods;
    foreach (var method in m) {
      if (method.Name == "Load") {
        Console.WriteLine("\nMethod: " + method.FullName);
        for (int i = 0; i < Math.Min(60, method.Body.Instructions.Count); i++) {
          Console.WriteLine("  " + method.Body.Instructions[i]);
        }
      }
    }
  }
}
