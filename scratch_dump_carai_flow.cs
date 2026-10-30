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
            if (m.Name == "SimulationStep" && m.Parameters.Count == 6)
            {
                Console.WriteLine($"=== CarAI.SimulationStep (6 params) ===");
                for (int i = 0; i < m.Body.Instructions.Count; i++)
                {
                    var inst = m.Body.Instructions[i];
                    var fr = inst.Operand as FieldReference;
                    if (fr != null && (fr.Name == "m_totalTrafficFlow" || fr.Name == "m_maxTrafficFlow"))
                    {
                        for (int j = Math.Max(0, i - 10); j <= i + 5; j++)
                            Console.WriteLine($"  IL_{m.Body.Instructions[j].Offset:X4}: {m.Body.Instructions[j].OpCode} {m.Body.Instructions[j].Operand}");
                    }
                }
            }
        }
    }
}
