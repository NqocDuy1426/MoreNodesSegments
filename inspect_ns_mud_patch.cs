using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        string mod = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/3702700108/NetworkSkins.dll";
        var asm = AssemblyDefinition.ReadAssembly(mod);
        var t = asm.MainModule.Types.FirstOrDefault(x => x.Name.Contains("SimulationManagerManagersUpdateDataPatch"));
        Console.WriteLine("Type: " + t.FullName);
        foreach (var m in t.Methods) {
            Console.WriteLine("  Method: " + m.FullName);
            if (m.HasBody) {
                foreach (var ins in m.Body.Instructions) {
                    Console.WriteLine("    " + ins.ToString());
                }
            }
        }
    }
}
