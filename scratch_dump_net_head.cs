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
                for (int i = 0; i < 50; i++)
                {
                    var inst = m.Body.Instructions[i];
                    Console.WriteLine($"IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                }
            }
        }
    }
}
