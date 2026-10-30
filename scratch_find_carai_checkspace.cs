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
            if (m.Body == null) continue;
            for (int i = 0; i < m.Body.Instructions.Count; i++)
            {
                var inst = m.Body.Instructions[i];
                if (inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt)
                {
                    var mr = inst.Operand as MethodReference;
                    if (mr != null && mr.Name == "CheckSpace")
                    {
                        Console.WriteLine("CarAI.{0} calls CheckSpace at IL_{1:X4}", m.Name, inst.Offset);
                        // Print surrounding instructions
                        int start = Math.Max(0, i - 10);
                        int end = Math.Min(m.Body.Instructions.Count - 1, i + 20);
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
