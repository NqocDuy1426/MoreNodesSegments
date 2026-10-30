using System;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/ColossalManaged.dll");
        var s = asm.MainModule.GetType("ColossalFramework.Singleton`1");
        Console.WriteLine("Singleton fields:");
        foreach (var f in s.Fields)
            Console.WriteLine("  " + f.Name + " : " + f.FieldType);
    }
}
