using System;
using Mono.Cecil;
class P {
  static void Main() {
    var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
    var sde = asm.MainModule.GetType("TrafficManager.Lifecycle.SerializableDataExtension");
    foreach (var m in sde.Methods) {
      if (m.Name == "Load" || m.Name.Contains("LoadData") || m.Name == "OnLoadData") {
        Console.WriteLine("\nMethod: " + m.FullName);
        if (m.HasBody) {
          foreach (var ins in m.Body.Instructions) Console.WriteLine("  " + ins);
        }
      }
    }
  }
}
