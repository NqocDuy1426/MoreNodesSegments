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
        var ser = data.Methods.First(m => m.Name == "Serialize");
        Console.WriteLine("NetManager.Data.Serialize has " + ser.Body.Instructions.Count + " instructions.");
        for (int i = 0; i < ser.Body.Instructions.Count; i++)
        {
            var inst = ser.Body.Instructions[i];
            if (inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt)
            {
                string s = inst.Operand.ToString();
                if (s.Contains("BeginWrite") || s.Contains("Write") || s.Contains("WriteObject"))
                {
                    Console.WriteLine("  " + inst);
                }
            }
        }
    }
}
