using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2911178252/RebalancedIndustriesRevisited.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "CSLModsCommon.Manager.SerializationManager");
        Console.WriteLine("=== SerializationManager ===");
        foreach (var m in type.Methods)
        {
            Console.WriteLine("Method: " + m.Name);
            if (m.HasBody)
            {
                foreach (var ins in m.Body.Instructions)
                {
                    if (ins.Operand != null && (ins.Operand.ToString().Contains("Serialize") || ins.Operand.ToString().Contains("Data") || ins.Operand.ToString().Contains("Save")))
                    {
                        Console.WriteLine("  " + ins);
                    }
                }
            }
        }
    }
}
