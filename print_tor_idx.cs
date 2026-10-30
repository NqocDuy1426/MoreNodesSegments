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
        foreach (var name in new string[] { "TurnOnRedManager", "TrafficMeasurementManager" })
        {
            var type = assembly.Modules.SelectMany(m => m.GetTypes()).FirstOrDefault(t => t.FullName.Contains(name));
            if (type != null)
            {
                Console.WriteLine("\n=== Type: " + type.FullName + " ===");
                foreach (var m in type.Methods.Where(meth => meth.Name.Contains("Index") || meth.Name.Contains("Get") || meth.Name.Contains("TurnOnRed") || meth.Name.Contains("SegmentDir")))
                {
                    if (m.HasBody && m.Parameters.Count <= 3)
                    {
                        Console.WriteLine("Method: " + m.Name + " (" + m.Parameters.Count + " params)");
                        for (int i = 0; i < Math.Min(25, m.Body.Instructions.Count); i++)
                        {
                            var instr = m.Body.Instructions[i];
                            Console.WriteLine(string.Format("  IL_{0:X4}: {1} {2}", instr.Offset, instr.OpCode, instr.Operand));
                        }
                    }
                }
            }
        }
    }
}
