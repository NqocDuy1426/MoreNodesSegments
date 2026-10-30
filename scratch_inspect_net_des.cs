using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory("/Users/nqocduy/CS_MoreNetworkCapacity_Renew");
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll", readerParams);

        var net = asm.MainModule.GetType("NetManager");
        var data = net.NestedTypes.First(t => t.Name == "Data");
        var des = data.Methods.First(m => m.Name == "Deserialize");
        Console.WriteLine("NetManager.Data.Deserialize lines: " + des.Body.Instructions.Count);
        foreach (var inst in des.Body.Instructions)
        {
            if (inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt || inst.OpCode == OpCodes.Ldfld || inst.OpCode == OpCodes.Ldelem_Any || inst.OpCode == OpCodes.Stelem_Any || inst.OpCode == OpCodes.Ldelem_I4 || inst.OpCode == OpCodes.Stelem_I4 || inst.OpCode == OpCodes.Ldelem_Ref || inst.OpCode == OpCodes.Stelem_Ref || inst.OpCode.Name.Contains("elem"))
            {
                Console.WriteLine("  " + inst);
            }
        }
    }
}
