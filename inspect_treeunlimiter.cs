using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        string dllPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/455403039/TreeUnlimiter.dll";
        var asm = AssemblyDefinition.ReadAssembly(dllPath);
        foreach (var t in asm.MainModule.Types) {
            Console.WriteLine("Type: " + t.FullName);
            foreach (var m in t.Methods) {
                if (m.Name.Contains("EnsureInit") || m.Name.Contains("UpdateData") || m.Name.Contains("ResetTreeMananger")) {
                    Console.WriteLine("  Method: " + m.FullName);
                }
            }
        }
    }
}
