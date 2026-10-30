using System;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll");
        foreach (var type in asm.MainModule.Types)
        {
            if (type.Name.Contains("Net") || type.Name.Contains("Deserialize"))
            {
                Console.WriteLine("Type: " + type.FullName);
                foreach (var m in type.Methods)
                {
                    if (m.Name.Contains("Deserialize") || m.Name.Contains("Patch") || m.Name.Contains("Ensure"))
                        Console.WriteLine("  Method: " + m.Name);
                }
            }
        }
    }
}
