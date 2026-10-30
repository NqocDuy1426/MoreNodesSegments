using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var vbm = asm.MainModule.GetType("TrafficManager.Manager.Impl.VehicleBehaviorManager");
        if (vbm != null) {
            foreach (var m in vbm.Methods) {
                if (m.Name.Contains("CalcMaxSpeed")) {
                    Console.WriteLine("Method: " + m.FullName);
                    if (m.HasBody) {
                        foreach (var inst in m.Body.Instructions) {
                            Console.WriteLine("  " + inst);
                        }
                    }
                }
            }
        }
    }
}
