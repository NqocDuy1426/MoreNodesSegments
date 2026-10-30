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
        var m = t.GetMethod("SimulationStep", new string[] { "System.UInt16", "CitizenInstance&", "CitizenInstance/Frame&", "System.Boolean" });
        if (m == null)
        {
            Console.WriteLine("SimulationStep not found!");
            return;
        }
        Console.WriteLine("Found HumanAI.SimulationStep. Searching for 2048 / ~2048...");
        for (int i = 0; i < m.Body.Instructions.Count; i++)
        {
            var inst = m.Body.Instructions[i];
            if (inst.OpCode == OpCodes.Ldc_I4 && ((int)inst.Operand == 2048 || (int)inst.Operand == ~2048 || (int)inst.Operand == -2049))
            {
                Console.WriteLine("Instruction {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand);
                // Print surrounding 10 instructions
                int start = Math.Max(0, i - 5);
                int end = Math.Min(m.Body.Instructions.Count - 1, i + 10);
                for (int j = start; j <= end; j++)
                {
                    Console.WriteLine("    {0}: {1} {2}", m.Body.Instructions[j].Offset.ToString("X4"), m.Body.Instructions[j].OpCode, m.Body.Instructions[j].Operand);
                }
            }
        }
    }
}
