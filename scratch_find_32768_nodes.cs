using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        foreach (var type in asm.MainModule.Types)
        {
            foreach (var m in type.Methods)
            {
                if (!m.HasBody) continue;
                for (int i = 0; i < m.Body.Instructions.Count; i++)
                {
                    var ins = m.Body.Instructions[i];
                    if (ins.OpCode == OpCodes.Ldc_I4 && (int)ins.Operand == 32768)
                    {
                        // Check if nearby has shl or div or NetNode
                        Console.WriteLine($"Found ldc.i4 32768 in {type.Name}.{m.Name} at offset {ins.Offset:X4}");
                        for (int k = Math.Max(0, i - 4); k <= Math.Min(m.Body.Instructions.Count - 1, i + 6); k++)
                        {
                            Console.WriteLine($"   {m.Body.Instructions[k]}");
                        }
                    }
                }
            }
        }
    }
}
