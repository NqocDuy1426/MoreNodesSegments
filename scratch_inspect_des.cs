using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("ColossalManaged.dll");
        var ds = asm.MainModule.GetType("ColossalFramework.IO.DataSerializer");
        foreach (var m in ds.Methods.Where(x => x.Name == "Deserialize")) {
            Console.WriteLine("Method: " + m.FullName);
            if (m.HasBody) {
                foreach (var inst in m.Body.Instructions) {
                    Console.WriteLine("  " + inst);
                }
            }
        }
    }
}

