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
        foreach (var m in type.Methods)
        {
            if (m.HasBody)
            {
                foreach (var ins in m.Body.Instructions)
                {
                    if (ins.Operand != null && ins.Operand.ToString().Contains("m_visited"))
                    {
                        Console.WriteLine($"Method {m.Name} uses m_visited at {ins}");
                    }
                }
            }
        }
    }
}
