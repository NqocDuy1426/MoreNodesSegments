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
        var type = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "TransferManagerCore.PathNode");
        var m = type.Methods.FirstOrDefault(meth => meth.Name == "FindRoadAccess");
        Console.WriteLine("=== FindRoadAccess full method ===");
        foreach (var ins in m.Body.Instructions.Take(80))
        {
            Console.WriteLine(ins);
        }
    }
}
