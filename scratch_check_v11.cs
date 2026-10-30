using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.GetType("CarAI");
        foreach (var m in type.Methods)
        {
            if (m.Name == "CalculateSegmentPosition")
            {
                Console.WriteLine("V_11 type: " + m.Body.Variables[11].VariableType.FullName);
                for (int i = 0; i < 35; i++)
                {
                    Console.WriteLine($"V_{i}: {m.Body.Variables[i].VariableType.FullName}");
                }
            }
        }
    }
}
