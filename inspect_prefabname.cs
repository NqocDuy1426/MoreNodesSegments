using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var pc = asm.MainModule.Types.FirstOrDefault(t => t.Name == "PrefabCollection`1");
        var m = pc.Methods.FirstOrDefault(x => x.Name == "PrefabName");
        Console.WriteLine("Method: " + m.FullName);
        foreach (var ins in m.Body.Instructions) {
            Console.WriteLine("  " + ins.ToString());
        }
    }
}
