using System;
using System.Linq;
using Mono.Cecil;
class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var t = asm.MainModule.GetType("ZoneManager");
        foreach (var f in t.Fields) {
            Console.WriteLine($"Field: {f.FieldType} {f.Name}");
        }
    }
}
