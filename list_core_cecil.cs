using System;
using Mono.Cecil;

public class ListCoreCecil
{
    public static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("MoreNetworkCapacity.Core.dll");
        Console.WriteLine("Types in MoreNetworkCapacity.Core.dll: " + asm.MainModule.Types.Count);
        foreach (var t in asm.MainModule.Types)
        {
            Console.WriteLine("Type: " + t.FullName);
            foreach (var m in t.Methods)
            {
                Console.WriteLine("  Method: " + m.FullName);
            }
        }
    }
}
