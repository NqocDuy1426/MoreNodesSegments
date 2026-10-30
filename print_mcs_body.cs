using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var vbm = asm.MainModule.GetType("TrafficManager.Manager.Impl.VehicleBehaviorManager");
        var m = vbm.Methods.FirstOrDefault(meth => meth.Name == "MayChangeSegment" && meth.Parameters.Count == 15);
        if (m != null) {
            Console.WriteLine("Method: " + m.FullName);
            foreach (var inst in m.Body.Instructions) {
                Console.WriteLine("  " + inst);
            }
        }
    }
}
