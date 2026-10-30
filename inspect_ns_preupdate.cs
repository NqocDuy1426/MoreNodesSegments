using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        string mod = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/3702700108/NetworkSkins.dll";
        var asm = AssemblyDefinition.ReadAssembly(mod);
        var t = asm.MainModule.Types.FirstOrDefault(x => x.Name == "NetworkSkinManager");
        var m = t.Methods.FirstOrDefault(x => x.Name == "OnPreUpdateData");
        Console.WriteLine("Method: " + m.FullName);
        foreach (var ins in m.Body.Instructions) {
            Console.WriteLine("  " + ins.ToString());
        }
    }
}
