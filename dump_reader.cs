
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("ColossalManaged.dll");
        var pr = asm.MainModule.GetType("ColossalFramework.Packaging.PackageReader");
        foreach (var m in pr.Methods) {
            Console.WriteLine("PackageReader: " + m.FullName);
        }
        var pd = asm.MainModule.GetType("ColossalFramework.Packaging.PackageDeserializer");
        foreach (var m in pd.Methods) {
            Console.WriteLine("PackageDeserializer: " + m.FullName);
        }
    }
}
