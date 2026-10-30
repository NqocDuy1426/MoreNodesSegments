using System;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/ColossalManaged.dll");
        var ea = asm.MainModule.GetType("ColossalFramework.IO.EncodedArray");
        Console.WriteLine("EncodedArray nested types:");
        foreach (var t in ea.NestedTypes)
        {
            Console.WriteLine("  " + t.Name);
            foreach (var m in t.Methods)
            {
                if (m.Name == "Read" || m.Name == "BeginRead" || m.Name == "EndRead")
                    Console.WriteLine("    " + m.Name);
            }
        }
    }
}
