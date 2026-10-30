using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/3829341972/MoreNodesSegmentsBeta.dll");
        var t = asm.MainModule.GetType("MoreNetworkCapacityRenew.RenewMod");
        var getter = t.Methods.First(m => m.Name == "get_Name");
        foreach (var inst in getter.Body.Instructions) {
            Console.WriteLine(inst);
        }
    }
}
