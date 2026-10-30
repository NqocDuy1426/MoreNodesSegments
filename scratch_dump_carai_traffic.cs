using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var gameDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");

        var type = asm.MainModule.GetType("CarAI");
        foreach (var m in type.Methods)
        {
            if (m.Name.Contains("SimulationStep"))
            {
                Console.WriteLine($"=== CarAI.{m.Name} ({m.Parameters.Count} params) ===");
                for (int i = 0; i < m.Body.Instructions.Count; i++)
                {
                    var inst = m.Body.Instructions[i];
                    var mr = inst.Operand as MethodReference;
                    if (mr != null && mr.Name == "AddTraffic")
                    {
                        for (int j = Math.Max(0, i - 8); j <= i + 2; j++)
                            Console.WriteLine($"  IL_{m.Body.Instructions[j].Offset:X4}: {m.Body.Instructions[j].OpCode} {m.Body.Instructions[j].Operand}");
                    }
                }
            }
        }
    }
}
