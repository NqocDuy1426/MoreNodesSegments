using System;
using Mono.Cecil;
class P {
  static void Main() {
    var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
    var cpf = asm.MainModule.GetType("TrafficManager.Custom.PathFinding.CustomPathFind");
    foreach (var m in cpf.Methods) {
      if (m.Name == "PathFindImplementation") {
        Console.WriteLine("\nMethod: " + m.FullName);
        for (int i = 0; i < m.Body.Instructions.Count; i++) {
          var ins = m.Body.Instructions[i];
          if (ins.Offset >= 0x3e0 && ins.Offset <= 0x460) {
            Console.WriteLine(string.Format("  IL_{0:X4}: {1}", ins.Offset, ins));
          }
        }
      }
    }
  }
}
