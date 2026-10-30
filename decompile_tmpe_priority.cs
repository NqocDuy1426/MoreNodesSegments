using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var resolver = new DefaultAssemblyResolver();
        string home = Environment.GetEnvironmentVariable("HOME");
        string gameDir = home + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        resolver.AddSearchDirectory(gameDir);
        var p = new ReaderParameters { AssemblyResolver = resolver };
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll", p);
        
        var tpm = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "TrafficManager.Manager.Impl.TrafficPriorityManager");
        if (tpm != null) {
            Console.WriteLine("TrafficPriorityManager methods:");
            foreach (var m in tpm.Methods.Where(m => m.Name == "HasPriority")) {
                Console.WriteLine("  " + m.FullName);
                if (m.HasBody) {
                    foreach (var inst in m.Body.Instructions) {
                        if (inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt || inst.OpCode == OpCodes.Ldfld || inst.OpCode == OpCodes.Stfld) {
                            Console.WriteLine("    " + inst.OpCode + " " + inst.Operand);
                        }
                    }
                }
            }
        }
    }
}
