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
            // Check for array length or comparisons or array accesses
            if (inst.OpCode == OpCodes.Ldlen || inst.OpCode.Name.Contains("blt") || inst.OpCode.Name.Contains("bge"))
            {
                // Print a small window around it
                int start = Math.Max(0, i - 4);
                int end = Math.Min(des.Body.Instructions.Count - 1, i + 3);
                Console.WriteLine("--- Loop/Branch at IL_" + inst.Offset.ToString("X4") + " ---");
                for (int j = start; j <= end; j++)
                {
                    Console.WriteLine("  " + (j == i ? "-> " : "   ") + des.Body.Instructions[j]);
                }
            }
        }
    }
}
