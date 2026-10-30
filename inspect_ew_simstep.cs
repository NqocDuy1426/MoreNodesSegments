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

        string[] types = new string[] { "ElectricityManager", "WaterManager" };
        foreach (var tName in types)
        {
            var t = asm.MainModule.GetType(tName);
            var m = t.Methods.FirstOrDefault(x => x.Name == "SimulationStepImpl");
            Console.WriteLine($"=== {tName}.SimulationStepImpl instructions: {m.Body.Instructions.Count} ===");
            foreach (var inst in m.Body.Instructions)
            {
                if (inst.OpCode == OpCodes.Ldc_I4 && Convert.ToInt32(inst.Operand) == 1000)
                {
                    Console.WriteLine($"  found 1000 at {inst.Offset:X4}");
                }
            }
        }
    }
}
