
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var t = asm.MainModule.GetType("TrafficManager.Manager.Impl.TrafficLightManager");
        foreach (var m in t.Methods) {
            Console.WriteLine("Method: " + m.Name);
            if (m.HasBody) {
                foreach (var ins in m.Body.Instructions) {
                    if (ins.OpCode.ToString().StartsWith("call") || ins.OpCode.ToString().StartsWith("ldfld") || ins.OpCode.ToString().StartsWith("stfld"))
                        Console.WriteLine("  " + ins);
                }
            }
        }
    }
}
