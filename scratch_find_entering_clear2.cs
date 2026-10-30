using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed");
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll", readerParams);

        var t = asm.MainModule.GetType("HumanAI");
        foreach (var m in t.Methods)
        {
            if (m.Name != "SimulationStep") continue;
            Console.WriteLine("Found HumanAI.SimulationStep (" + m.Parameters.Count + " params)");
            if (m.Body == null) continue;
            for (int i = 0; i < m.Body.Instructions.Count; i++)
            {
                var inst = m.Body.Instructions[i];
                if (inst.OpCode == OpCodes.Ldc_I4 && ((int)inst.Operand == 2048 || (int)inst.Operand == ~2048 || (int)inst.Operand == -2049))
                {
                    Console.WriteLine("Instruction {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand);
                    int start = Math.Max(0, i - 4);
                    int end = Math.Min(m.Body.Instructions.Count - 1, i + 8);
                    for (int j = start; j <= end; j++)
                    {
                        Console.WriteLine("    {0}: {1} {2}", m.Body.Instructions[j].Offset.ToString("X4"), m.Body.Instructions[j].OpCode, m.Body.Instructions[j].Operand);
                    }
                }
            }
        }
    }
}
