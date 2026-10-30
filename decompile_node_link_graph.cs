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
        var type = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "TransferManagerCore.NodeLinkGraph");
        if (type == null) { Console.WriteLine("NodeLinkGraph not found"); return; }

        foreach (var m in type.Methods)
        {
            Console.WriteLine("Method: " + m.Name);
            if (m.HasBody)
            {
                foreach (var ins in m.Body.Instructions)
                {
                    if (ins.OpCode == OpCodes.Newarr || (ins.Operand != null && (ins.Operand.ToString().Contains("32768") || ins.Operand.ToString().Contains("36864") || ins.Operand.ToString().Contains("65024") || ins.Operand.ToString().Contains("65536"))))
                    {
                        Console.WriteLine("  " + ins.Previous + " -> " + ins);
                    }
                }
            }
        }
    }
}
