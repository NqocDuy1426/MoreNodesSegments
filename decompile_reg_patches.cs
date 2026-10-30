using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2911178252/RebalancedIndustriesRevisited.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "RebalancedIndustriesRevisited.Managers.ModManager");
        var m = type.Methods.FirstOrDefault(meth => meth.Name == "RegisterPatches");
        Console.WriteLine("=== RegisterPatches ===");
        foreach (var ins in m.Body.Instructions) Console.WriteLine("  " + ins);
    }
}
