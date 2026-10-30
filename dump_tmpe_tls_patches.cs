using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class DumpTMPETlsPatches
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(tmpePath);
        string[] targetTypes = {
            "TrafficManager.Patch._RoadBaseAI.GetTrafficLightNodeStatePatch",
            "TrafficManager.Patch._RoadBaseAI.SetTrafficLightStatePatch",
            "TrafficManager.Patch._RoadBaseAI.TrafficLightSimulationStepPatch"
        };
        foreach (var tName in targetTypes)
        {
            var t = asm.MainModule.GetType(tName);
            if (t != null)
            {
                Console.WriteLine("================ " + tName + " ================");
                foreach (var m in t.Methods)
                {
                    Console.WriteLine("METHOD: " + m.FullName);
                    if (m.HasBody)
                    {
                        foreach (var inst in m.Body.Instructions)
                        {
                            Console.WriteLine(string.Format("  {0,-10} {1,-12} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand));
                        }
                    }
                }
            }
        }
    }
}
