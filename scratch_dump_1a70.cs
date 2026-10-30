using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var nm = asm.MainModule.GetType("NetManager");
        TypeDefinition data = null;
        foreach (var t in nm.NestedTypes)
        {
            if (t.Name == "Data") { data = t; break; }
        }
        foreach (var m in data.Methods)
        {
            if (m.Name == "Deserialize")
            {
                for (int i = 0; i < m.Body.Instructions.Count; i++)
                {
                    var inst = m.Body.Instructions[i];
                    if (inst.Offset >= 0x1A60 && inst.Offset <= 0x1AB5)
                    {
                        Console.WriteLine($"IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                    }
                }
            }
        }
    }
}
