using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        foreach (var t in asm.MainModule.Types) {
            if (t.Name.Contains("VehicleJunctionTransitState")) {
                Console.WriteLine("Type: " + t.FullName);
                foreach (var f in t.Fields) {
                    if (f.IsStatic) Console.WriteLine("  " + f.Name + " = " + f.Constant);
                }
            }
        }
    }
}
