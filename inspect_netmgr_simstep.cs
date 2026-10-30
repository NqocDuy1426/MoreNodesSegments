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

        var netMgr = asm.MainModule.GetType("NetManager");
        var m = netMgr.Methods.FirstOrDefault(x => x.Name == "SimulationStepImpl");
        Console.WriteLine($"NetManager.SimulationStepImpl instructions: {m.Body.Instructions.Count}");
        foreach (var inst in m.Body.Instructions)
        {
            if (inst.OpCode == OpCodes.Ldarg_1 || inst.OpCode == OpCodes.Ldc_I4 || inst.OpCode == OpCodes.Beq || inst.OpCode == OpCodes.Bne_Un)
            {
                Console.WriteLine($"   {inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
            }
        }
    }
}
