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

        foreach (var type in asm.MainModule.Types.Where(t => t.FullName.Contains("Patches")))
        {
            Console.WriteLine("=== " + type.FullName + " ===");
            foreach (var m in type.Methods)
            {
                Console.WriteLine("  Method: " + m.Name);
                if (m.HasBody)
                {
                    foreach (var ins in m.Body.Instructions)
                    {
                        if (ins.OpCode == OpCodes.Call || ins.OpCode == OpCodes.Callvirt)
                        {
                            if (ins.Operand.ToString().Contains("Patch") || ins.Operand.ToString().Contains("GetMethod"))
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
