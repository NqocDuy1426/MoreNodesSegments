using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var ti = asm.MainModule.Types.FirstOrDefault(x => x.Name == "TreeInstance");
        var m = ti.Methods.FirstOrDefault(x => x.Name == "UpdateTree");
        Console.WriteLine("TreeInstance UpdateTree: " + (m != null ? m.FullName : "null"));
        if (m != null) {
            foreach (var ins in m.Body.Instructions) {
                Console.WriteLine("  " + ins.ToString());
            }
        }
    }
}
