using System;
using System.IO;
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
        using (var sw = new StreamWriter("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/net_des_full.txt"))
        {
            foreach (var inst in des.Body.Instructions)
            {
                sw.WriteLine(string.Format("IL_{0:X4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand));
            }
        }
        Console.WriteLine("Dumped " + des.Body.Instructions.Count + " instructions.");
    }
}
