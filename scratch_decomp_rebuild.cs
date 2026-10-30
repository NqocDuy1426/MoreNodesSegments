using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("MoreNetworkCapacity.Core.dll");
        var c = asm.MainModule.GetType("MoreNetworkCapacity.NetManagerCustomizer");
        foreach (var m in c.Methods)
        {
            if (m.Name.Contains("Rebuild"))
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
