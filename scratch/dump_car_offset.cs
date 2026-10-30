using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var assembly = AssemblyDefinition.ReadAssembly(path);
        var type = assembly.MainModule.GetType("CarAI");
        foreach (var m in type.Methods)
        {
            if (m.Name == "CalculateSegmentPosition" && m.Parameters.Count >= 5)
            {
                foreach (var ins in m.Body.Instructions)
                {
                    if (ins.Offset >= 0x03f0 && ins.Offset <= 0x0448)
                    {
                        Console.WriteLine("  " + ins.ToString());
                    }
                }
            }
        }
    }
}
