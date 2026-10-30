using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var t = asm.MainModule.GetType("MoreNetworkCapacity.Compatibility.StaticBufferCompatibility");
        foreach (var m in t.Methods)
        {
            if (m.Name.Contains("SetStaticArray"))
            {
                Console.WriteLine($"=== {m.Name} ===");
                foreach (var inst in m.Body.Instructions)
                {
                    Console.WriteLine($"  {inst.OpCode} {inst.Operand}");
                }
            }
        }
    }
}
