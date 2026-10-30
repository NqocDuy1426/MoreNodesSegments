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

        var type = asm.MainModule.GetType("NetManager");
        foreach (var m in type.Methods)
        {
            if (m.Name == "SimulationStepImpl")
            {
                Console.WriteLine($"=== NetManager.{m.Name} ===");
                for (int i = 0; i < m.Body.Instructions.Count; i++)
                {
                    var inst = m.Body.Instructions[i];
                    if (inst.OpCode == OpCodes.Ldfld)
                    {
                        var fr = inst.Operand as FieldReference;
                        if (fr != null && (fr.Name.Contains("m_segments") || fr.Name.Contains("m_nodes")))
                        {
                            for (int j = Math.Max(0, i - 4); j <= i + 6; j++)
                                Console.WriteLine($"  IL_{m.Body.Instructions[j].Offset:X4}: {m.Body.Instructions[j].OpCode} {m.Body.Instructions[j].Operand}");
                        }
                    }
                }
            }
        }
    }
}
