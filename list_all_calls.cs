using System;
using System.Linq;
using Mono.Cecil;

class ListAllCalls
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var netMgr = asm.MainModule.Types.FirstOrDefault(t => t.Name == "NetManager");
        var netMgrData = netMgr.NestedTypes.FirstOrDefault(t => t.Name == "Data");
        var des = netMgrData.Methods.FirstOrDefault(m => m.Name == "Deserialize");
        Console.WriteLine("NetManager.Data.Deserialize calls:");
        foreach (var inst in des.Body.Instructions)
        {
            if (inst.OpCode.ToString().StartsWith("call"))
            {
                Console.WriteLine(string.Format("  {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand));
            }
        }
    }
}
