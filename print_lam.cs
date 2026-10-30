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
        var type = assembly.Modules.SelectMany(m => m.GetTypes()).FirstOrDefault(t => t.FullName == "TrafficManager.Manager.Impl.LaneArrowManager");
        if (type != null)
        {
            Console.WriteLine("Type: " + type.FullName);
            foreach (var f in type.Fields)
            {
                Console.WriteLine("  Field: " + f.FieldType + " " + f.Name);
            }
            foreach (var m in type.Methods)
            {
                Console.WriteLine("Method: " + m.Name + " (" + m.Parameters.Count + " params)");
                if (m.HasBody && (m.Name.Contains("Arrow") || m.Name.Contains("Recalculate") || m.Name.Contains("Lane") || m.Name.Contains("Load")))
                {
                    for (int i = 0; i < Math.Min(35, m.Body.Instructions.Count); i++)
                    {
                        var instr = m.Body.Instructions[i];
                        Console.WriteLine(string.Format("  IL_{0:X4}: {1} {2}", instr.Offset, instr.OpCode, instr.Operand));
                    }
                }
            }
        }
    }
}
