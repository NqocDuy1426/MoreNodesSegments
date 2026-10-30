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
            if (type.FullName.Contains("RoadAccess") || type.FullName.Contains("PathDistance") || type.FullName.Contains("PathfindFailure"))
            {
                Console.WriteLine("=== TYPE: " + type.FullName + " ===");
                foreach (var m in type.Methods)
                {
                    Console.WriteLine("  METHOD: " + m.Name);
                    if (m.HasBody)
                    {
                        foreach (var ins in m.Body.Instructions)
                        {
                            if (ins.Operand != null && (ins.Operand.ToString().Contains("NetManager") || ins.Operand.ToString().Contains("PathManager") || ins.Operand.ToString().Contains("m_segments") || ins.Operand.ToString().Contains("m_nodes")))
                            {
                                Console.WriteLine("    " + ins);
                            }
                        }
                    }
                }
            }
        }
    }
}
