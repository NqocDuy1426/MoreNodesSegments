using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var jrm = asm.MainModule.GetType("TrafficManager.Manager.Impl.JunctionRestrictionsManager");
        var jr = jrm.NestedTypes.FirstOrDefault(t => t.Name == "JunctionRestrictions");
        foreach (var m in jr.Methods) {
            Console.WriteLine("Method: " + m.FullName);
            foreach (var inst in m.Body.Instructions) {
                Console.WriteLine("  " + inst);
            }
        }
    }
}
