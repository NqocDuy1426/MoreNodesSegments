using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.Types.FirstOrDefault(t => t.FullName.Contains("CustomPathManager"));

        var m1 = type.Methods.FirstOrDefault(m => m.Name == "CustomCreatePath");
        Console.WriteLine("=== CustomCreatePath ===");
        foreach (var ins in m1.Body.Instructions) Console.WriteLine("  " + ins);

        var m2 = type.Methods.FirstOrDefault(m => m.Name == "SimulationStepImpl");
        Console.WriteLine("=== SimulationStepImpl ===");
        foreach (var ins in m2.Body.Instructions.Take(80)) Console.WriteLine("  " + ins);
    }
}
