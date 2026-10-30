using System;
using Mono.Cecil;
class P {
  static void Main() {
    var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
    var cpf = asm.MainModule.GetType("TrafficManager.Custom.PathFinding.CustomPathFind");
    foreach (var m in cpf.Methods) {
      if (m.Name == "ProcessItemMain") {
        Console.WriteLine(m.FullName);
        foreach (var p in m.Parameters) Console.WriteLine($"  {p.ParameterType} {p.Name}");
      }
    }
  }
}
