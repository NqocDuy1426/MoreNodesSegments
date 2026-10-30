using System;
using System.IO;
using Mono.Cecil;

class Program {
    static void Main(string[] args) {
        string dll = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/ColossalManaged.dll";
        var asm = AssemblyDefinition.ReadAssembly(dll);
        var ds = asm.MainModule.GetType("ColossalFramework.IO.DataSerializer");
        Console.WriteLine("=== DataSerializer Methods ===");
        foreach (var m in ds.Methods) {
            Console.WriteLine($"  {m.ReturnType.FullName} {m.Name}({string.Join(", ", m.Parameters)})");
        }
    }
}
