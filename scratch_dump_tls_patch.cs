using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(tmpePath);
        var type = asm.MainModule.GetType("TrafficManager.Patch._RoadBaseAI.TrafficLightSimulationStepPatch");
        Console.WriteLine("Type found: " + (type != null));
        if (type != null)
        {
            foreach (var m in type.Methods)
            {
                Console.WriteLine("Method: " + m.FullName);
                if (m.HasBody)
                {
                    foreach (var ins in m.Body.Instructions)
                    {
                        Console.WriteLine("  " + ins.ToString());
                    }
                }
            }
        }
    }
}
