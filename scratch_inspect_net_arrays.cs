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
        var data = net.NestedTypes.First(t => t.Name == "Data");
        var des = data.Methods.First(m => m.Name == "Deserialize");

        for (int i = 0; i < des.Body.Instructions.Count; i++)
        {
            var inst = des.Body.Instructions[i];
            if (inst.OpCode.Name.Contains("elem") || inst.OpCode == OpCodes.Ldfld)
            {
                if (inst.OpCode.Name.Contains("elem"))
                {
                    // Print context
                    int start = Math.Max(0, i - 6);
                    int end = Math.Min(des.Body.Instructions.Count - 1, i + 2);
                    Console.WriteLine("--- Array access at IL_" + inst.Offset.ToString("X4") + " (" + inst.OpCode + ") ---");
                    for (int j = start; j <= end; j++)
                    {
                        Console.WriteLine("  " + (j == i ? "-> " : "   ") + des.Body.Instructions[j]);
                    }
                }
            }
        }
    }
}
