
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("ColossalManaged.dll");
        var pkg = asm.MainModule.GetType("ColossalFramework.Packaging.Package");
        foreach (var m in pkg.Methods) {
            if (m.IsConstructor && m.Parameters.Count == 3) {
                Console.WriteLine("Ctor: " + m.FullName);
                foreach (var inst in m.Body.Instructions) {
                    Console.WriteLine("  " + inst);
                }
            }
        }
    }
}
