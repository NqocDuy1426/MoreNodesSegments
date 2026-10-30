using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll");
        var net = asm.MainModule.GetType("NetManager");
        var initNode = net.Methods.First(m => m.Name == "InitializeNode");
        Console.WriteLine("NetManager.InitializeNode instructions:");
        foreach (var inst in initNode.Body.Instructions)
        {
            Console.WriteLine("  " + inst);
        }

        var initSeg = net.Methods.First(m => m.Name == "InitializeSegment");
        Console.WriteLine("\nNetManager.InitializeSegment instructions:");
        foreach (var inst in initSeg.Body.Instructions)
        {
            Console.WriteLine("  " + inst);
        }
    }
}
