using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

class DumpNodeState
{
    static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(Path.Combine(gameDir, "Assembly-CSharp.dll"));
        var roadBaseAI = asm.MainModule.GetType("RoadBaseAI");
        foreach (var m in roadBaseAI.Methods.Where(x => x.Name == "GetTrafficLightNodeState"))
        {
            Console.WriteLine("METHOD: " + m.FullName);
            foreach (var p in m.Parameters)
            {
                Console.WriteLine("  " + p.ParameterType.FullName + " " + p.Name);
            }
        }
    }
}
