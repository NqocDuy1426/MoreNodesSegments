using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("ColossalManaged.dll");
        var ds = asm.MainModule.Types.FirstOrDefault(t => t.Name == "DataSerializer");
        var m = ds.Methods.FirstOrDefault(x => x.Name == "ReadSharedString");
        Console.WriteLine("Method: " + m.FullName);
        foreach (var ins in m.Body.Instructions) {
            Console.WriteLine("  " + ins.ToString());
        }
    }
}
