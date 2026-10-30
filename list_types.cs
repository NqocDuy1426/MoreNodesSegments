using System;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("MoreNetworkCapacityRenew.dll");
        Console.WriteLine("=== Current MoreNetworkCapacityRenew.dll Types ===");
        foreach (var t in asm.MainModule.Types.OrderBy(x => x.FullName))
        {
            if (t.Name.StartsWith("<")) continue;
            Console.WriteLine("TYPE: " + t.FullName);
        }
    }
}
