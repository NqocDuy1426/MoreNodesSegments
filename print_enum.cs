using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var en = asm.MainModule.GetType("TrafficManager.API.Traffic.Enums.VehicleJunctionTransitState");
        if (en != null) {
            foreach (var f in en.Fields) {
                if (f.IsStatic) {
                    Console.WriteLine(f.Name + " = " + f.Constant);
                }
            }
        }
    }
}
