
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("ColossalManaged.dll");
        foreach (var t in asm.MainModule.Types) {
            if (t.Namespace.Contains("Packaging")) Console.WriteLine("Type: " + t.FullName);
        }
    }
}
