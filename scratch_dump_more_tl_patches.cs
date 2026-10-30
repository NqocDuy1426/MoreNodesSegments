using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(tmpePath);
        string[] patchNames = new string[] {
            "TrafficManager.Patch._RoadBaseAI.SetTrafficLightStatePatch",
            "TrafficManager.Patch._RoadBaseAI.GetTrafficLightNodeStatePatch",
            "TrafficManager.Patch._VehicleAI._CarAI.SimulationStepPatch"
        };
        foreach (var p in patchNames)
        {
            var type = asm.MainModule.GetType(p);
            Console.WriteLine("Type: " + p + " (found: " + (type != null) + ")");
            if (type != null)
            {
                foreach (var m in type.Methods)
                {
                    Console.WriteLine("  Method: " + m.FullName);
                    if (m.HasBody)
                    {
                        foreach (var ins in m.Body.Instructions)
                        {
                            Console.WriteLine("    " + ins.ToString());
                        }
                    }
                }
            }
        }
    }
}
