using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.GetType("RoadBaseAI");
        MethodDefinition target = null;
        foreach (var m in type.Methods) if (m.Name == "TrafficLightSimulationStep") target = m;

        Console.WriteLine("Variables count: " + target.Body.Variables.Count);
        for (int i = 0; i < target.Body.Variables.Count; i++)
        {
            var v = target.Body.Variables[i];
            Console.WriteLine($"V_{i}: {v.VariableType.FullName}");
        }
    }
}
