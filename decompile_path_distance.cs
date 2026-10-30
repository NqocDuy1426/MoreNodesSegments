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

        foreach (var type in asm.MainModule.Types.Where(t => t.FullName.Contains("PathDistance") || t.FullName.Contains("PathNode") || t.FullName.Contains("PathCandidate")))
        {
            Console.WriteLine("=== TYPE: " + type.FullName + " ===");
            foreach (var m in type.Methods)
            {
                if (m.Name == ".ctor" || m.Name == ".cctor" || m.Name.Contains("Reset") || m.Name.Contains("Init") || m.Name.Contains("Generate"))
                {
                    Console.WriteLine("  METHOD: " + m.Name);
                    if (m.HasBody)
                    {
                        foreach (var ins in m.Body.Instructions)
                        {
                            if (ins.OpCode == OpCodes.Newarr || (ins.Operand != null && (ins.Operand.ToString().Contains("32768") || ins.Operand.ToString().Contains("36864") || ins.Operand.ToString().Contains("49152") || ins.Operand.ToString().Contains("65536") || ins.Operand.ToString().Contains("65024"))))
                            {
                                Console.WriteLine("    " + ins.Previous + " -> " + ins);
                            }
                        }
                    }
                }
            }
        }
    }
}
