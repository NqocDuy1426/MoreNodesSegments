using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var t = asm.MainModule.GetType("TrafficManager.Patch._CommonBuildingAI.SimulationStepPatch");
        if (t != null)
        {
            Console.WriteLine("Found " + t.FullName);
            foreach (var m in t.Methods)
            {
                Console.WriteLine("Method: " + m.FullName);
                if (m.Body != null)
                {
                    foreach (var inst in m.Body.Instructions)
                    {
                        Console.WriteLine("  {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand);
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("Not found!");
        }
    }
}
