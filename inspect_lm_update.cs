using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var lm = asm.MainModule.Types.FirstOrDefault(t => t.Name == "LoadingManager");
        var m = lm.Methods.FirstOrDefault(t => t.Name == "Update");
        Console.WriteLine("LoadingManager Update: " + (m != null ? m.FullName : "null"));
        if (m != null) {
            foreach (var ins in m.Body.Instructions) {
                Console.WriteLine("  " + ins.ToString());
            }
        }
    }
}
