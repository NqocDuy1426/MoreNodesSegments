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

        var type = assembly.Modules.SelectMany(m => m.Types).FirstOrDefault(t => t.FullName == "TrafficManager.Patch.PatchCommons");
        var method = type.Methods.FirstOrDefault(m => m.Name == "GetModifiedBlockCounterInstructions");

        if (method != null)
        {
            foreach (var instr in method.Body.Instructions)
            {
                Console.WriteLine(string.Format("IL_{0:X4}: {1} {2}", instr.Offset, instr.OpCode, instr.Operand));
            }
        }
    }
}
