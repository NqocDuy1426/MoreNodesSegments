
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("ColossalManaged.dll");
        var ds = asm.MainModule.GetType("ColossalFramework.IO.DataSerializer");
        foreach (var nested in ds.NestedTypes) {
            if (nested.Name == "Mode") {
                foreach (var f in nested.Fields) {
                    if (f.IsStatic) Console.WriteLine("Mode." + f.Name);
                }
            }
        }
    }
}
