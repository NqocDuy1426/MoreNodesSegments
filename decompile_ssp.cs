using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.Types.FirstOrDefault(t => t.FullName.Contains("SimulationStepPatch") && t.FullName.Contains("CommonBuildingAI"));
        Console.WriteLine($"Type: {type.FullName}");

        foreach (var m in type.Methods)
        {
            Console.WriteLine($"Method: {m.Name} ({string.Join(", ", m.Parameters.Select(p => p.ParameterType.Name))})");
            foreach (var ins in m.Body.Instructions)
            {
                Console.WriteLine($"  {ins.Offset:X4}: {ins}");
            }
        }
    }
}
