using System;
using Mono.Cecil;
class P {
  static void Main() {
    var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
    var cpf = asm.MainModule.GetType("TrafficManager.Custom.PathFinding.CustomPathFind");
    foreach (var f in cpf.Fields) {
      if (f.Name.Contains("savedGameOptions") || f.Name.Contains("Options") || f.Name.Contains("options")) {
        Console.WriteLine("Field: " + f.Name + " (" + f.FieldType + ")");
      }
    }
    foreach (var m in cpf.Methods) {
      if (m.Name == "PathFindImplementation") {
        for (int i = 0; i < 30; i++) {
          Console.WriteLine("  " + m.Body.Instructions[i]);
        }
      }
    }
  }
}
