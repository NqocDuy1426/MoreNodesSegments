using System;
using Mono.Cecil;
class P {
  static void Main() {
    var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
    var log = asm.MainModule.GetType("CSUtil.Commons.Log");
    Console.WriteLine("Type: " + log.FullName);
    foreach (var m in log.Methods) {
      if (m.Name == "Info" || m.Name == "Error") {
        Console.WriteLine("\nMethod: " + m.FullName);
        for (int i = 0; i < Math.Min(20, m.Body.Instructions.Count); i++) {
          Console.WriteLine("  " + m.Body.Instructions[i]);
        }
      }
    }
  }
}
