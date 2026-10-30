using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var tm = asm.MainModule.Types.FirstOrDefault(x => x.Name == "TreeManager");
        var m = tm.Methods.FirstOrDefault(x => x.Name == "ReleaseTreeImplementation");
        Console.WriteLine("TM ReleaseTreeImplementation: " + (m != null ? m.FullName : "null"));
        if (m != null) {
            foreach (var ins in m.Body.Instructions) {
                Console.WriteLine("  " + ins.ToString());
            }
        }
    }
}
