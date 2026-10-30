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
        var m = type.Methods.FirstOrDefault(meth => meth.Name == "FindNearestNeighborId");
        foreach (var ins in m.Body.Instructions.Where(i => i.Offset >= 0x0190 && i.Offset <= 0x01e0))
        {
            Console.WriteLine(ins);
        }
    }
}
