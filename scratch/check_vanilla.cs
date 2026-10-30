using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var assembly = AssemblyDefinition.ReadAssembly(path);
        
        DumpMethod(assembly, "RoadBaseAI", "TrafficLightSimulationStep");
    }

    static void DumpMethod(AssemblyDefinition asm, string typeName, string methodName)
    {
        Console.WriteLine("========================================");
        Console.WriteLine("TYPE: " + typeName + ", METHOD: " + methodName);
        Console.WriteLine("========================================");
        var type = asm.MainModule.GetType(typeName);
        if (type == null)
        {
            Console.WriteLine("Type not found: " + typeName);
            return;
        }
        foreach (var m in type.Methods)
        {
            if (m.Name == methodName)
            {
                Console.WriteLine("--- " + m.ToString() + " ---");
                if (m.Body != null)
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
