using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var t = asm.MainModule.GetType("CarAI");
        // Vehicle.Flags.WaitingPath is 0x4 (4)
        // Vehicle.Flags.WaitingSpace is 0x8 (8)
        foreach (var m in t.Methods)
        {
            if (m.Name.Contains("SimulationStep"))
            {
                Console.WriteLine("CarAI." + m.Name);
                if (m.Body != null)
                {
                    for (int i = 0; i < m.Body.Instructions.Count; i++)
                    {
                        var inst = m.Body.Instructions[i];
                        if (inst.OpCode == OpCodes.Ldc_I4 && ((int)inst.Operand == 4 || (int)inst.Operand == 8 || (int)inst.Operand == 12))
                        {
                            Console.WriteLine("  {0:X4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand);
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
