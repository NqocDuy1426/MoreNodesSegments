
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("ColossalManaged.dll");
        var pm = asm.MainModule.GetType("ColossalFramework.Packaging.PackageManager");
        foreach (var m in pm.Methods) {
            if (m.IsStatic) Console.WriteLine("Static Method: " + m.FullName);
        }
    }
}
