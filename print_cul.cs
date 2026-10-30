using System;
using Mono.Cecil;
class P {
  static void Main() {
    var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
    var cpf = asm.MainModule.GetType("TrafficManager.Custom.PathFinding.CustomPathFind");
    foreach (var m in cpf.Methods) {
      if (m.Name == "CanUseLane") {
        Console.WriteLine("\nMethod: " + m.FullName);
        foreach (var ins in m.Body.Instructions) Console.WriteLine("  " + ins);
      }
    }
  }
}
