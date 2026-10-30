using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        foreach (var t in asm.MainModule.Types)
        {
            foreach (var m in t.Methods)
            {
                if (m.Name.Contains("TMPE") || m.Name.Contains("Buffer") || m.Name.Contains("Static"))
                {
                    Console.WriteLine($"{t.FullName}.{m.Name}");
                }
            }
        }
    }
}
