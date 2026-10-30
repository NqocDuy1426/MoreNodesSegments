using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("MoreNetworkCapacity.Core.dll");
        foreach (var t in asm.MainModule.Types)
        {
            if (t.Name.IndexOf("color", StringComparison.OrdinalIgnoreCase) >= 0 ||
                t.Name.IndexOf("render", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Console.WriteLine($"Core type: {t.Name}");
            }
        }
    }
}
