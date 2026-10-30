using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/ColossalManaged.dll");
        var ds = asm.MainModule.GetType("ColossalFramework.IO.DataSerializer");
        var m = ds.Methods.First(x => x.Name == "ReadSharedType" && x.Parameters.Count == 1);
        Console.WriteLine("ReadSharedType(handler) instructions:");
        foreach (var inst in m.Body.Instructions)
            Console.WriteLine("  " + inst);
    }
}
