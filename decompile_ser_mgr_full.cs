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
        var type = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "CSLModsCommon.Manager.SerializationManager");
        var m1 = type.Methods.FirstOrDefault(m => m.Name == "SerializeData");
        Console.WriteLine("=== SerializeData ===");
        foreach (var ins in m1.Body.Instructions) Console.WriteLine("  " + ins);

        var m2 = type.Methods.FirstOrDefault(m => m.Name == "DeserializeData");
        Console.WriteLine("=== DeserializeData ===");
        foreach (var ins in m2.Body.Instructions) Console.WriteLine("  " + ins);
    }
}
