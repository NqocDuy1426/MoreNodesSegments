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

        PrintMethod(assembly, "TrafficManager.Manager.Impl.VehicleBehaviorManager", "MayDespawn");
        PrintMethod(assembly, "TrafficManager.Patch._VehicleAI._CarAI.SimulationStepPatch", "Prefix");
    }

    static void PrintMethod(AssemblyDefinition asm, string typeName, string methodName)
    {
        Console.WriteLine("================ " + typeName + " :: " + methodName + " ================");
        var type = asm.Modules.SelectMany(m => m.Types).FirstOrDefault(t => t.FullName == typeName);
        if (type == null) { Console.WriteLine("Type not found!"); return; }
        var method = type.Methods.FirstOrDefault(m => m.Name == methodName);
        if (method == null) { Console.WriteLine("Method not found!"); return; }

        if (!method.HasBody) { Console.WriteLine("No body!"); return; }
        foreach (var instr in method.Body.Instructions)
        {
            Console.WriteLine(string.Format("IL_{0:X4}: {1} {2}", instr.Offset, instr.OpCode, instr.Operand));
        }
    }
}
