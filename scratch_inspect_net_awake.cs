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
        var awake = net.Methods.First(m => m.Name == "Awake");
        foreach (var inst in awake.Body.Instructions)
        {
            if (inst.OpCode == OpCodes.Ldfld || inst.OpCode == OpCodes.Stfld || inst.OpCode == OpCodes.Newarr)
                Console.WriteLine("  " + inst);
        }
    }
}
