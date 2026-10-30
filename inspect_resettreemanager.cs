using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        string dllPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/455403039/TreeUnlimiter.dll";
        var asm = AssemblyDefinition.ReadAssembly(dllPath);
        var t = asm.MainModule.Types.FirstOrDefault(x => x.Name == "LoadingExtension");
        var m = t.Methods.FirstOrDefault(x => x.Name == "ResetTreeMananger");
        Console.WriteLine("Method: " + m.FullName);
        foreach (var ins in m.Body.Instructions) {
            Console.WriteLine("  " + ins.ToString());
        }
    }
}
