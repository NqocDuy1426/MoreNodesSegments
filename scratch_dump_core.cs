using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        foreach (var type in asm.MainModule.Types)
        {
            Console.WriteLine("Core Type: " + type.FullName);
            foreach (var m in type.Methods)
            {
                if (m.IsPublic || m.Name.Contains("Patch") || m.Name.Contains("Transpiler"))
                {
                    Console.WriteLine("  " + m.Name);
                }
            }
        }
    }
}
