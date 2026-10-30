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
        foreach (var type in asm.MainModule.Types)
        {
            ScanType(type);
        }
    }

    static void ScanType(TypeDefinition type)
    {
        foreach (var nested in type.NestedTypes) ScanType(nested);

        foreach (var m in type.Methods)
        {
            if (m.HasBody)
            {
                foreach (var ins in m.Body.Instructions)
                {
                    if (ins.OpCode == OpCodes.Ldc_I4)
                    {
                        int val = (int)ins.Operand;
                        if (val == 32768 || val == 36864 || val == 49152 || val == 16384 || val == 262144 || val == 524288)
                        {
                            Console.WriteLine($"Constant {val} in: {type.FullName}.{m.Name} at {ins}");
                        }
                    }
                }
            }
        }
    }
}
