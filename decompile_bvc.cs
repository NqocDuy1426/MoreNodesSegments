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
        var type = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "TransferManagerCore.BuildingVehicleCount");
        if (type == null) { Console.WriteLine("Type not found"); return; }
        foreach (var m in type.Methods)
        {
            Console.WriteLine("Method: " + m.Name);
            if (m.HasBody)
            {
                foreach (var ins in m.Body.Instructions.Where(i => i.OpCode == OpCodes.Ldc_I4 && (int)i.Operand == 16384))
                {
                    Console.WriteLine("  16384 at: " + ins);
                }
            }
        }
    }
}
