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
            if (t.Name.StartsWith("Array32"))
            {
                Console.WriteLine("Type: " + t.FullName);
                foreach (var f in t.Fields)
                {
                    Console.WriteLine("  Field: " + f.Name + " (" + f.FieldType.FullName + ")");
                }
                foreach (var m in t.Methods)
                {
                    if (m.Name == "ReleaseItem" || m.Name == "ClearUnused")
                    {
                        Console.WriteLine("=== Method: " + m.Name + " ===");
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
