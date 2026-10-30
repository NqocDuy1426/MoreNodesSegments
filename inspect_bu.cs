using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        string dllPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/2105755179/Patches/BehaviourUpdater.dll";
        var asm = AssemblyDefinition.ReadAssembly(dllPath);
        foreach (var t in asm.MainModule.Types) {
            Console.WriteLine("Type: " + t.FullName);
            foreach (var m in t.Methods) {
                Console.WriteLine("  Method: " + m.FullName);
                if (m.HasBody) {
                    foreach (var ins in m.Body.Instructions) {
                        if (ins.Operand != null) {
                            string op = ins.Operand.ToString();
                            if (op.Contains("LoadingManager") || op.Contains("FpsBoosterUpdate") || op.Contains("Material")) {
                                Console.WriteLine("    " + ins.ToString());
                            }
                        }
                    }
                }
            }
        }
    }
}
