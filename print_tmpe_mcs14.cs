using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string tmpePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var assembly = AssemblyDefinition.ReadAssembly(tmpePath);
        var type = assembly.Modules.SelectMany(m => m.GetTypes()).FirstOrDefault(t => t.FullName == "TrafficManager.Manager.Impl.VehicleBehaviorManager");
        var meth = type.Methods.FirstOrDefault(m => m.Name == "MayChangeSegment" && m.Parameters.Count == 14);
        using (var sw = new StreamWriter("mcs14_il.txt"))
        {
            for (int i = 0; i < meth.Body.Instructions.Count; i++)
            {
                var instr = meth.Body.Instructions[i];
                sw.WriteLine(string.Format("IL_{0:X4}: {1} {2}", instr.Offset, instr.OpCode, instr.Operand));
            }
        }
        Console.WriteLine("Written " + meth.Body.Instructions.Count + " instructions for MayChangeSegment 14-params.");
    }
}
