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

        foreach (var type in asm.MainModule.Types.Where(t => t.FullName.Contains("Patches")))
        {
            var patchMethod = type.Methods.FirstOrDefault(m => m.Name == "Patch");
            if (patchMethod != null)
            {
                Console.WriteLine("=== Patch method in: " + type.FullName + " ===");
                foreach (var ins in patchMethod.Body.Instructions)
                {
                    if (ins.OpCode == OpCodes.Ldtoken || ins.OpCode == OpCodes.Ldstr)
                    {
                        Console.WriteLine("    " + ins);
                    }
                }
            }
        }
    }
}
