using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

class DumpRoadBaseAINodeSimstep
{
    static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(Path.Combine(gameDir, "Assembly-CSharp.dll"));
        var type = asm.MainModule.GetType("RoadBaseAI");
        foreach (var m in type.Methods.Where(x => x.Name == "SimulationStep"))
        {
            if (m.Parameters.Count == 2 && m.Parameters[1].ParameterType.FullName.Contains("NetNode"))
            {
                Console.WriteLine(m.FullName);
                foreach (var inst in m.Body.Instructions)
                {
                    Console.WriteLine(string.Format("  {0,-10} {1,-12} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand));
                }
            }
        }
    }
}
