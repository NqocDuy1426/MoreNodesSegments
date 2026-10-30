using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string p = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(p);

        var type = asm.MainModule.GetType("TrafficManager.Patch._RoadBaseAI.SegmentSimulationStepPatch");
        foreach (var m in type.Methods)
        {
            Console.WriteLine($"=== {m.Name} ===");
            if (m.Body != null)
            {
                foreach (var inst in m.Body.Instructions)
                    Console.WriteLine($"  IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
            }
        }
    }
}
