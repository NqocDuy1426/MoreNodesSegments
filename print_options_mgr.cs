using System;
using Mono.Cecil;
class P {
  static void Main() {
    var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
    var om = asm.MainModule.GetType("TrafficManager.Manager.Impl.OptionsManager");
    Console.WriteLine("Type: " + om.FullName);
    foreach (var m in om.Methods) {
      Console.WriteLine("\nMethod: " + m.FullName);
      if (m.HasBody) {
        foreach (var ins in m.Body.Instructions) Console.WriteLine("  " + ins);
      }
    }
  }
}
