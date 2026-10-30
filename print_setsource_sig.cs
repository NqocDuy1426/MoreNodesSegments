using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/2911178252/RebalancedIndustriesRevisited.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "RebalancedIndustriesRevisited.Patches.CargoCapacityPatch");
        var m = type.Methods.FirstOrDefault(meth => meth.Name == "SetSourcePrefix");
        Console.WriteLine("SetSourcePrefix (" + string.Join(", ", m.Parameters.Select(p => p.ParameterType.FullName + " " + p.Name)) + ")");
    }
}
