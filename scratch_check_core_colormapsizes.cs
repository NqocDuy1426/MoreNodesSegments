using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("MoreNetworkCapacity.Core.dll");
        var t = asm.MainModule.GetType("MoreNetworkCapacity.Capacity");
        foreach (var m in t.Methods)
        {
            if (m.Name.Contains("Color"))
            {
                Console.WriteLine($"=== Capacity.{m.Name} ===");
                foreach (var inst in m.Body.Instructions)
                    Console.WriteLine($"  IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
            }
        }
    }
}
