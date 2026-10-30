using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        string dllPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/2858591409/LoadingScreenModRevisited.dll";
        var asm = AssemblyDefinition.ReadAssembly(dllPath);
        foreach (var t in asm.MainModule.Types) {
            foreach (var m in t.Methods) {
                if (m.HasBody) {
                    foreach (var ins in m.Body.Instructions) {
                        if (ins.Operand != null && ins.Operand.ToString().Contains("ReportMissingAssets")) {
                            Console.WriteLine($"{t.FullName}::{m.Name} -> {ins}");
                        }
                    }
                }
            }
        }
    }
}
