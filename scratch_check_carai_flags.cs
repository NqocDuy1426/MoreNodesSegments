using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var t = asm.MainModule.GetType("CarAI");
        foreach (var m in t.Methods)
        {
            if (m.Name == "SimulationStep" && m.Parameters.Count == 6)
            {
                for (int i = 0; i < m.Body.Instructions.Count; i++)
                {
                    var inst = m.Body.Instructions[i];
                    if (inst.OpCode == OpCodes.Ldc_I4)
                    {
                        int val = (int)inst.Operand;
                        if (val == 256 || val == 65536 || val == 131072 || val == 33554432 || val == 524288)
                        {
                            Console.WriteLine("Instruction {0:X4}: ldc.i4 {1} (0x{1:X})", inst.Offset, val);
                            int start = Math.Max(0, i - 2);
                            int end = Math.Min(m.Body.Instructions.Count - 1, i + 10);
                            for (int j = start; j <= end; j++)
                            {
                                Console.WriteLine("    {0:X4}: {1} {2}", m.Body.Instructions[j].Offset, m.Body.Instructions[j].OpCode, m.Body.Instructions[j].Operand);
                            }
                        }
                    }
                }
            }
        }
    }
}
