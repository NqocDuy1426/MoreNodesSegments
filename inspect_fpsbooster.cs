using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        string dllPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/2105755179/FPS_Booster.dll";
        var asm = AssemblyDefinition.ReadAssembly(dllPath);
        foreach (var t in asm.MainModule.Types) {
            foreach (var m in t.Methods) {
                if (m.Name.Contains("Update") || m.Name.Contains("FpsBooster")) {
                    Console.WriteLine($"{t.FullName}::{m.FullName}");
                }
            }
        }
    }
}
