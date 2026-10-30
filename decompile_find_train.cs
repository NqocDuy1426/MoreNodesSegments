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
        
        var t1 = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "TransferManagerCore.CheckRoadAccessPatches");
        var m1 = t1.Methods.FirstOrDefault(m => m.Name == "FindTrainAccess");
        Console.WriteLine("=== FindTrainAccess (lines around 36864) ===");
        foreach (var ins in m1.Body.Instructions.Where(i => i.Offset >= 0x00e0 && i.Offset <= 0x0120))
        {
            Console.WriteLine(ins);
        }

        var t2 = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "TransferManagerCore.PathNode");
        var m2 = t2.Methods.FirstOrDefault(m => m.Name == "FindRoadAccess");
        Console.WriteLine("=== FindRoadAccess (lines around 36864) ===");
        foreach (var ins in m2.Body.Instructions.Where(i => i.Offset >= 0x00e0 && i.Offset <= 0x0120))
        {
            Console.WriteLine(ins);
        }

        var t3 = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "TransferManagerCore.NodeLinkGenerator");
        var m3 = t3.Methods.FirstOrDefault(m => m.Name == "ProcessLaneNodes");
        Console.WriteLine("=== ProcessLaneNodes (lines around 32768) ===");
        foreach (var ins in m3.Body.Instructions.Where(i => i.Offset >= 0x0040 && i.Offset <= 0x0080))
        {
            Console.WriteLine(ins);
        }

        var m4 = t3.Methods.FirstOrDefault(m => m.Name == "ProcessLaneSegments");
        Console.WriteLine("=== ProcessLaneSegments (lines around 32768) ===");
        foreach (var ins in m4.Body.Instructions.Where(i => i.Offset >= 0x0020 && i.Offset <= 0x0060))
        {
            Console.WriteLine(ins);
        }
    }
}
