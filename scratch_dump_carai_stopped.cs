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

        var t = asm.MainModule.GetType("CarAI");
        foreach (var m in t.Methods)
        {
            if (m.Name == "SimulationStep" && m.Parameters.Count == 6)
            {
                Console.WriteLine("--- CarAI.SimulationStep (6 params) ---");
                for (int i = 0; i < m.Body.Instructions.Count; i++)
                {
                    var inst = m.Body.Instructions[i];
                    if (inst.OpCode == OpCodes.Ldc_I4 && (int)inst.Operand == 512)
                    {
                        Console.WriteLine("Found 512 at {0:X4}", inst.Offset);
                        int start = Math.Max(0, i - 2);
                        int end = Math.Min(m.Body.Instructions.Count - 1, i + 25);
                        for (int j = start; j <= end; j++)
                        {
                            Console.WriteLine("  {0}: {1} {2}", m.Body.Instructions[j].Offset.ToString("X4"), m.Body.Instructions[j].OpCode, m.Body.Instructions[j].Operand);
                        }
                    }
                }
            }
        }
    }
}
