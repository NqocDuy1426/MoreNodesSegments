using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.GetType("RoadBaseAI");
        foreach (var m in type.Methods)
        {
            if (m.Name == "SetTrafficLightState" || m.Name == "GetTrafficLightState")
            {
                Console.WriteLine(m.FullName);
                for (int i = 0; i < m.Parameters.Count; i++)
                {
                    var p = m.Parameters[i];
                    Console.WriteLine($"   P{i}: {p.Name} : {p.ParameterType.FullName} (IsByRef: {p.ParameterType.IsByReference})");
                }
            }
        }
    }
}
