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

        var lm = asm.MainModule.GetType("LoadingManager");
        foreach (var nested in lm.NestedTypes)
        {
            var moveNext = nested.Methods.FirstOrDefault(m => m.Name == "MoveNext");
            if (moveNext != null)
            {
                foreach (var inst in moveNext.Body.Instructions)
                {
                    if (inst.OpCode == OpCodes.Ldfld && inst.Operand != null && inst.Operand.ToString().Contains("m_simulationDataLoaded"))
                    {
                        Console.WriteLine($"Found wait for m_simulationDataLoaded in {nested.FullName} at {inst.Offset:X4}");
                    }
                }
            }
        }
    }
}
