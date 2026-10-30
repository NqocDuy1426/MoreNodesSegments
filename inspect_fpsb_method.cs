using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        string dllPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(dllPath);
        var lm = asm.MainModule.Types.FirstOrDefault(t => t.Name == "LoadingManager");
        foreach (var m in lm.Methods) {
            if (m.Name.Contains("FpsBooster")) {
                Console.WriteLine("Found method: " + m.FullName);
                if (m.HasBody) {
                    foreach (var ins in m.Body.Instructions) {
                        Console.WriteLine("  " + ins.ToString());
                    }
                }
            }
        }
    }
}
