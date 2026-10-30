using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods/3797247466/MoreTransportLines.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        foreach (var t in asm.MainModule.Types)
        {
            if (t.Name.Contains("Deserialize") || t.Name.Contains("Serialize") || t.Name.Contains("CapacityTools"))
            {
                Console.WriteLine("\n=== Type: " + t.FullName + " ===");
                foreach (var m in t.Methods)
                {
                    Console.WriteLine("--- Method: " + m.Name + " ---");
                    if (m.HasBody)
                    {
                        foreach (var instr in m.Body.Instructions)
                        {
                            Console.WriteLine($"  IL_{instr.Offset:x4}: {instr.OpCode} {instr.Operand}");
                        }
                    }
                }
            }
        }
    }
}
