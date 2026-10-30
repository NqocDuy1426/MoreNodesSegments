using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var t = asm.MainModule.Types.FirstOrDefault(x => x.Name == "TreeInstance");
        var m = t.Methods.FirstOrDefault(x => x.Name == "get_Info");
        Console.WriteLine("Method: " + m.FullName);
        foreach (var ins in m.Body.Instructions) {
            Console.WriteLine("  " + ins.ToString());
        }
    }
}
