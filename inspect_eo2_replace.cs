using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string eoPath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2862121823/EightyOne2.dll";
        var assembly = AssemblyDefinition.ReadAssembly(eoPath);
        var type = assembly.MainModule.GetType("EightyOne2.Patches.DistrictManagerPatches");
        foreach (var m in type.Methods)
        {
            if (m.Name == "ReplaceDistrictConstants")
            {
                Console.WriteLine("=== Method: " + m.FullName + " ===");
                foreach (var inst in m.Body.Instructions)
                {
                    Console.WriteLine("  " + inst.OpCode + " " + inst.Operand);
                }
            }
        }
    }
}
