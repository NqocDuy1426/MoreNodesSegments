using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var jrm = asm.MainModule.GetType("TrafficManager.Manager.Impl.JunctionRestrictionsManager");
        foreach (var m in jrm.Methods.Where(meth => meth.Name.Contains("UpdateDefaults"))) {
            Console.WriteLine("Method: " + m.FullName);
            foreach (var inst in m.Body.Instructions) {
                Console.WriteLine("  " + inst);
            }
        }
    }
}
