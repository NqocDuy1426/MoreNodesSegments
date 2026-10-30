using System;
using Mono.Cecil;
class P {
  static void Main() {
    var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
    var cpf = asm.MainModule.GetType("TrafficManager.Custom.PathFinding.CustomPathFind");
    foreach (var m in cpf.Methods) {
      if (m.HasBody) {
        foreach (var ins in m.Body.Instructions) {
          if (ins.Operand != null && ins.Operand.ToString().Contains("GetGameSpeedLimit")) {
            Console.WriteLine($"{m.Name} -> {ins}");
          }
        }
      }
    }
  }
}
