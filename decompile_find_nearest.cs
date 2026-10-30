using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Desktop/CSL_Mods_Backup_16bit_FINAL/Local_2804719780/TransferManagerCE.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "TransferManagerCore.PathDistance");
        var m = type.Methods.FirstOrDefault(meth => meth.Name == "UpdateNode");
        Console.WriteLine("=== UpdateNode ===");
        foreach (var ins in m.Body.Instructions)
        {
            Console.WriteLine(ins);
        }

        m = type.Methods.FirstOrDefault(meth => meth.Name == "FindNearestNeighborId");
        Console.WriteLine("=== FindNearestNeighborId ===");
        foreach (var ins in m.Body.Instructions.Take(50))
        {
            Console.WriteLine(ins);
        }
    }
}
