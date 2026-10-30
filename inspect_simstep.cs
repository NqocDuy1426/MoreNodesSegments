using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var gameManaged = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(gameManaged);
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };
        var asm = AssemblyDefinition.ReadAssembly(Path.Combine(gameManaged, "Assembly-CSharp.dll"), readerParams);

        string[] managers = new string[] {
            "NetManager", "BuildingManager", "TreeManager", "ElectricityManager", 
            "WaterManager", "TransportManager", "PathManager", "VehicleManager", "CitizenManager"
        };

        foreach (var mgrName in managers)
        {
            var type = asm.MainModule.GetType(mgrName);
            if (type == null) continue;
            var m = type.Methods.FirstOrDefault(x => x.Name == "SimulationStep" && x.Parameters.Count == 1);
            if (m != null)
            {
                Console.WriteLine($"=== {mgrName}.SimulationStep (params: {m.Parameters.Count}) ===");
                foreach (var inst in m.Body.Instructions)
                {
                    Console.WriteLine($"  {inst.OpCode} {inst.Operand}");
                }
            }
        }
    }
}
