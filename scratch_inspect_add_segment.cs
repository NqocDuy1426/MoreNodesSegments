using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll");
        var node = asm.MainModule.GetType("NetNode");
        var m = node.Methods.First(x => x.Name == "AddSegment");
        Console.WriteLine("NetNode.AddSegment instructions:");
        foreach (var inst in m.Body.Instructions)
            Console.WriteLine("  " + inst);
    }
}
