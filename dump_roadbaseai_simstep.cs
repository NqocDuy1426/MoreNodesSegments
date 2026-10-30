using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

class DumpRoadBaseAISimstep
{
    static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(Path.Combine(gameDir, "Assembly-CSharp.dll"));
        var type = asm.MainModule.GetType("RoadBaseAI");
        var m = type.Methods.FirstOrDefault(x => x.Name == "SimulationStep" && x.Parameters.Count == 2);
        if (m != null)
        {
            foreach (var inst in m.Body.Instructions)
            {
                Console.WriteLine(string.Format("  {0,-10} {1,-12} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand));
            }
        }
    }
}
