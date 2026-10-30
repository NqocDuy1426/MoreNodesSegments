using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string origPath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/3814019901/MoreNetworkCapacity.dll";
        var assembly = AssemblyDefinition.ReadAssembly(origPath);
        var type = assembly.MainModule.GetType("MoreNetworkCapacity.Patches.OnLevelLoadedPatch");
        foreach (var m in type.Methods)
        {
            Console.WriteLine("=== Method: " + m.FullName + " ===");
            foreach (var inst in m.Body.Instructions)
            {
                Console.WriteLine("  " + inst.OpCode + " " + inst.Operand);
            }
        }
    }
}
