
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("ColossalManaged.dll");
        var pkg = asm.MainModule.GetType("ColossalFramework.Packaging.Package");
        foreach (var m in pkg.Methods) {
            if (m.IsStatic) Console.WriteLine("Static Method: " + m.FullName);
        }
    }
}
