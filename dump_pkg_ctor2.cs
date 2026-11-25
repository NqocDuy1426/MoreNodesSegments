
using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("ColossalManaged.dll");
        var pkg = asm.MainModule.GetType("ColossalFramework.Packaging.Package");
        foreach (var m in pkg.Methods) {
            if (m.IsConstructor && m.Parameters.Count == 2) {
                Console.WriteLine("Ctor: " + m.FullName);
                foreach (var inst in m.Body.Instructions) {
                    Console.WriteLine("  " + inst);
                }
            }
        }
    }
}
