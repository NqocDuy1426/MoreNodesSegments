using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var tm = asm.MainModule.Types.FirstOrDefault(x => x.Name == "TreeManager");
        var m = tm.Methods.FirstOrDefault(x => x.Name == "ReleaseTree");
        Console.WriteLine("TM ReleaseTree: " + (m != null ? m.FullName : "null"));
        if (m != null) {
            foreach (var ins in m.Body.Instructions) {
                Console.WriteLine("  " + ins.ToString());
            }
        }
        string dllPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/455403039/TreeUnlimiter.dll";
        var uasm = AssemblyDefinition.ReadAssembly(dllPath);
        var ut = uasm.MainModule.Types.FirstOrDefault(x => x.Name == "LimitTreeManager");
        var um = ut.Methods.FirstOrDefault(x => x.Name == "ReleaseTree");
        Console.WriteLine("UT LimitTreeManager ReleaseTree: " + (um != null ? um.FullName : "null"));
        if (um != null) {
            foreach (var ins in um.Body.Instructions) {
                Console.WriteLine("  " + ins.ToString());
            }
        }
    }
}
