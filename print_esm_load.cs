using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string tmpePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var assembly = AssemblyDefinition.ReadAssembly(tmpePath);
        var type = assembly.Modules.SelectMany(m => m.GetTypes()).FirstOrDefault(t => t.FullName == "TrafficManager.Manager.Impl.ExtSegmentManager");
        if (type != null)
        {
            Console.WriteLine("ExtSegmentManager methods:");
            foreach (var m in type.Methods.Where(meth => meth.Name.Contains("Load") || meth.Name.Contains("Recalculate") || meth.Name.Contains("Init")))
            {
                Console.WriteLine("\nMethod: " + m.Name);
                if (m.HasBody)
                {
                    for (int i = 0; i < m.Body.Instructions.Count; i++)
                    {
                        var instr = m.Body.Instructions[i];
                        if (instr.OpCode == OpCodes.Ldc_I4 || instr.OpCode == OpCodes.Ldc_I4_S || instr.Operand?.ToString().Contains("36864") == true || instr.Operand?.ToString().Contains("65024") == true)
                        {
                            Console.WriteLine(string.Format("  IL_{0:X4}: {1} {2}", instr.Offset, instr.OpCode, instr.Operand));
                        }
                    }
                }
            }
        }
    }
}
