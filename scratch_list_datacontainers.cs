using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        foreach (var t in asm.MainModule.Types) {
            foreach (var n in t.NestedTypes) {
                if (n.Name == "Data" && n.Interfaces.Any(i => i.InterfaceType.Name.Contains("IDataContainer"))) {
                    Console.WriteLine($"\"{t.Name}\",");
                }
            }
        }
    }
}
