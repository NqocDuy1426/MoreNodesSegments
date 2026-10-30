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

        var nm = asm.MainModule.GetType("NetManager");
        var m = nm.Methods.FirstOrDefault(x => x.Name == "SimulationStepImpl");
        Console.WriteLine("Instructions checking subStep in NetManager.SimulationStepImpl:");
        for (int i = 0; i < m.Body.Instructions.Count; i++)
        {
            var inst = m.Body.Instructions[i];
            if (inst.OpCode == OpCodes.Ldarg_1)
            {
                for (int j = i; j <= Math.Min(m.Body.Instructions.Count - 1, i + 6); j++)
                {
                    Console.WriteLine($"   {m.Body.Instructions[j].Offset:X4}: {m.Body.Instructions[j].OpCode} {m.Body.Instructions[j].Operand}");
                }
                Console.WriteLine("---");
            }
        }
    }
}
