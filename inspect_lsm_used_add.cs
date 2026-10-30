using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        string dllPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/2858591409/LoadingScreenModRevisited.dll";
        var asm = AssemblyDefinition.ReadAssembly(dllPath);
        var t = asm.MainModule.Types.FirstOrDefault(x => x.Name == "UsedAssets");
        foreach (var m in t.Methods) {
            Console.WriteLine("Method: " + m.FullName);
            if (m.HasBody) {
                foreach (var ins in m.Body.Instructions) {
                    if (ins.OpCode == OpCodes.Callvirt && ins.Operand.ToString().Contains("Add")) {
                        Console.WriteLine("  " + ins.ToString());
                    }
                }
            }
        }
    }
}
