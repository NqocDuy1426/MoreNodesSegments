using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string corePath = "/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll";
        var assembly = AssemblyDefinition.ReadAssembly(corePath);
        Console.WriteLine("Core assembly: " + assembly.FullName);
        foreach (var t in assembly.MainModule.Types)
        {
            if (t.Name.Contains("Patch") || t.Name.Contains("Net") || t.Name.Contains("Manager"))
            {
                Console.WriteLine("Type: " + t.FullName);
            }
        }
    }
}
