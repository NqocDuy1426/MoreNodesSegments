using System;
using System.Linq;
using Mono.Cecil;
class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("ColossalManaged.dll");
        var t = asm.MainModule.GetType("ColossalFramework.IO.DataSerializer");
        foreach (var c in t.Methods.Where(x => x.IsConstructor)) {
            Console.WriteLine($"Ctor: {c.FullName}");
            foreach (var p in c.Parameters) Console.WriteLine($"   {p.ParameterType} {p.Name}");
        }
        foreach (var f in t.Fields) {
            Console.WriteLine($"Field: {f.FieldType} {f.Name}");
        }
    }
}
