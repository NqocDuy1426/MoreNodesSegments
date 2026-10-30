using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "CarAI");
        foreach (var m in type.Methods.Where(m => m.Name == "SimulationStep"))
        {
            Console.WriteLine($"CarAI.{m.Name} ({string.Join(", ", m.Parameters.Select(p => p.ParameterType.Name))})");
        }
        type = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "VehicleAI");
        foreach (var m in type.Methods.Where(m => m.Name == "SimulationStep"))
        {
            Console.WriteLine($"VehicleAI.{m.Name} ({string.Join(", ", m.Parameters.Select(p => p.ParameterType.Name))})");
        }
    }
}
