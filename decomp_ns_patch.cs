using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/3702700108/NetworkSkins.dll");
        foreach (var t in asm.MainModule.Types) {
            if (t.FullName.Contains("SimulationManagerManagersUpdateDataPatch") || t.FullName.Contains("ManagersUpdateData")) {
                Console.WriteLine("Type: " + t.FullName);
                foreach (var m in t.Methods) {
                    Console.WriteLine("  Method: " + m.FullName);
                    if (m.HasBody) {
                        foreach (var i in m.Body.Instructions) {
                            Console.WriteLine("    " + i);
                        }
                    }
                }
            }
        }
    }
}
