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

        var type = assembly.Modules.SelectMany(m => m.Types).FirstOrDefault(t => t.FullName == "TrafficManager.Manager.Impl.ExtVehicleManager");
        var method = type.Methods.FirstOrDefault(m => m.Name == "UpdateVehiclePosition" && m.Parameters.Count == 2 && m.Parameters[0].ParameterType.Name.Contains("Vehicle&"));

        if (method != null)
        {
            var instrs = method.Body.Instructions.Skip(30).Take(45);
            foreach (var instr in instrs)
            {
                Console.WriteLine(string.Format("IL_{0:X4}: {1} {2}", instr.Offset, instr.OpCode, instr.Operand));
            }
        }
    }
}
