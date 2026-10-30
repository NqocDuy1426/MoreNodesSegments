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
        var t1 = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "RebalancedIndustriesRevisited.Patches.ExtractingFacilityAIPatch");
        var m1 = t1.Methods.FirstOrDefault(m => m.Name == "GetOutputLoad");
        Console.WriteLine("=== ExtractingFacilityAIPatch.GetOutputLoad ===");
        foreach (var ins in m1.Body.Instructions) Console.WriteLine("  " + ins);

        var t2 = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "RebalancedIndustriesRevisited.Patches.ProcessingFacilityAIPatch");
        var m2 = t2.Methods.FirstOrDefault(m => m.Name == "GetOutputLoad");
        Console.WriteLine("=== ProcessingFacilityAIPatch.GetOutputLoad ===");
        foreach (var ins in m2.Body.Instructions) Console.WriteLine("  " + ins);

        var m3 = t2.Methods.FirstOrDefault(m => m.Name == "GetInputLoad");
        Console.WriteLine("=== ProcessingFacilityAIPatch.GetInputLoad ===");
        foreach (var ins in m3.Body.Instructions) Console.WriteLine("  " + ins);
    }
}
