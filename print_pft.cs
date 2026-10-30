using System;
using Mono.Cecil;
class P {
  static void Main() {
    var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
    var cpf = asm.MainModule.GetType("TrafficManager.Custom.PathFinding.CustomPathFind");
    foreach (var m in cpf.Methods) {
      if (m.Name == "PathFindThread") {
        Console.WriteLine("\nMethod: " + m.FullName);
        foreach (var h in m.Body.ExceptionHandlers) {
          Console.WriteLine($"Handler: {h.HandlerType} CatchType: {h.CatchType} Start: IL_{h.HandlerStart.Offset:X4}");
        }
        for (int i = 0; i < m.Body.Instructions.Count; i++) {
          var ins = m.Body.Instructions[i];
          if (ins.ToString().Contains("Path find error") || ins.ToString().Contains("FLAG_FAILED") || ins.ToString().Contains("PathFindImplementation")) {
            Console.WriteLine(string.Format("  IL_{0:X4}: {1}", ins.Offset, ins));
          }
        }
      }
    }
  }
}
