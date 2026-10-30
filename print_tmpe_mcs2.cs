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

        var type = assembly.Modules.SelectMany(mod => mod.Types).FirstOrDefault(t => t.FullName == "TrafficManager.Manager.Impl.VehicleBehaviorManager");
        var meth = type.Methods.FirstOrDefault(candidate => candidate.Name == "MayChangeSegment" && candidate.Parameters.Count == 15);
        if (meth != null)
        {
            for (int i = 0; i < 30; i++)
            {
                var instr = meth.Body.Instructions[i];
                Console.WriteLine(string.Format("IL_{0:X4}: {1} {2}", instr.Offset, instr.OpCode, instr.Operand));
            }
        }
    }
}
