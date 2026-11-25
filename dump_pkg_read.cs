
using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("ColossalManaged.dll");
        var pr = asm.MainModule.GetType("ColossalFramework.Packaging.Package");
        foreach (var m in pr.Methods) {
            if (m.Name.Contains("Filter") || m.Name.Contains("Load") || m.Name.Contains("Open") || m.Name.Contains("Read")) {
                Console.WriteLine("Method: " + m.FullName);
            }
        }
    }
}
