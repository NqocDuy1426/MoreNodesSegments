using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll");
        var type = asm.MainModule.Types.First(t => t.FullName == "MoreNetworkCapacity.Patches.NetManagerDeserializePatch");
        var m = type.Methods.First(x => x.Name == "Postfix");
        Console.WriteLine("NetManagerDeserializePatch.Postfix instructions:");
        foreach (var inst in m.Body.Instructions)
            Console.WriteLine("  " + inst);
    }
}
