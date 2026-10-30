using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var gameManaged = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(gameManaged);
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };
        var asm = AssemblyDefinition.ReadAssembly(Path.Combine(gameManaged, "Assembly-CSharp.dll"), readerParams);

        foreach (var type in asm.MainModule.Types)
        {
            if (type.Interfaces.Any(i => i.InterfaceType.Name == "ISimulationManager"))
            {
                Console.WriteLine($"ISimulationManager: {type.FullName}");
                foreach (var m in type.Methods.Where(m => m.Name.Contains("SimulationStep") || m.Name.Contains("LateUpdateData")))
                {
                    Console.WriteLine($"   method: {m.Name} ({string.Join(", ", m.Parameters.Select(p => p.ParameterType.Name))})");
                }
            }
        }
    }
}
