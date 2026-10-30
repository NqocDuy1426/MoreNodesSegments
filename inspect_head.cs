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
            Console.WriteLine($"=== {tName}.SimulationStepImpl head ===");
            for (int i = 0; i < Math.Min(30, m.Body.Instructions.Count); i++)
            {
                var inst = m.Body.Instructions[i];
                Console.WriteLine($"  {inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
            }
        }
    }
}
