using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var tpm = asm.MainModule.GetType("TrafficManager.Manager.Impl.TrafficPriorityManager");
        foreach (var m in tpm.Methods) {
            if (m.Name == "HasPriority") {
                Console.WriteLine("Method: " + m.FullName);
                foreach (var inst in m.Body.Instructions) {
                    Console.WriteLine("  " + inst);
                }
            }
        }
    }
}
