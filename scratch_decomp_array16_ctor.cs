using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        foreach (var t in asm.MainModule.Types)
        {
            if (t.Name.StartsWith("Array16"))
            {
                foreach (var m in t.Methods)
                {
                    if (m.IsConstructor)
                    {
                        Console.WriteLine("=== Ctor ===");
                        foreach (var inst in m.Body.Instructions)
                        {
                            Console.WriteLine($"  IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                        }
                    }
                }
            }
        }
    }
}
