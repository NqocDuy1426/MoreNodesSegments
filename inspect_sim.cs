using System;
using System.IO;
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
        var simMgr = asm.MainModule.GetType("SimulationManager");
        Console.WriteLine("SimulationManager methods:");
        foreach (var m in simMgr.Methods)
        {
            if (m.Name.Contains("LateUpdateData"))
            {
                Console.WriteLine("--- Method: " + m.FullName + " ---");
                foreach (var inst in m.Body.Instructions)
                {
                    Console.WriteLine("  " + inst.OpCode + " " + inst.Operand);
                }
            }
        }
    }
}
