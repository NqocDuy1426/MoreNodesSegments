using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        string dllPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/2858591409/LoadingScreenModRevisited.dll";
        var asm = AssemblyDefinition.ReadAssembly(dllPath);
        var t = asm.MainModule.Types.FirstOrDefault(x => x.Name == "Reports");
        var m = t.Methods.FirstOrDefault(x => x.Name == "AddMissing");
        Console.WriteLine("Method: " + m.FullName);
        foreach (var ins in m.Body.Instructions) {
            Console.WriteLine("  " + ins.ToString());
        }
    }
}
