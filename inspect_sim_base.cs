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

        var smBase = asm.MainModule.Types.FirstOrDefault(t => t.Name.StartsWith("SimulationManagerBase`"));
        if (smBase != null)
        {
            Console.WriteLine("Found " + smBase.FullName);
            foreach (var m in smBase.Methods)
            {
                Console.WriteLine("Method: " + m.FullName);
                if (m.Name.Contains("SimulationStep"))
                {
                    foreach (var inst in m.Body.Instructions)
                    {
                        Console.WriteLine("   " + inst.OpCode + " " + inst.Operand);
                    }
                }
            }
        }
    }
}
