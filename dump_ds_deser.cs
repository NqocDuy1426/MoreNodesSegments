
using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("ColossalManaged.dll");
        var ds = asm.MainModule.GetType("ColossalFramework.IO.DataSerializer");
        foreach (var m in ds.Methods) {
            if (m.Name == "Deserialize" && m.HasGenericParameters) {
                Console.WriteLine("Generic Method: " + m.FullName);
                foreach (var inst in m.Body.Instructions) {
                    Console.WriteLine("  " + inst);
                }
            }
        }
    }
}
