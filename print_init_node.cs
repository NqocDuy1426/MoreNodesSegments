using System;
using System.Linq;
using Mono.Cecil;

class PrintInitNode
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var netMgr = asm.MainModule.Types.FirstOrDefault(t => t.Name == "NetManager");
        var mInitNode = netMgr.Methods.FirstOrDefault(m => m.Name == "InitializeNode");
        Console.WriteLine("NetManager.InitializeNode:");
        foreach (var inst in mInitNode.Body.Instructions)
        {
            Console.WriteLine(string.Format("  {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand));
        }

        var mInitSeg = netMgr.Methods.FirstOrDefault(m => m.Name == "InitializeSegment");
        Console.WriteLine("\nNetManager.InitializeSegment:");
        foreach (var inst in mInitSeg.Body.Instructions)
        {
            Console.WriteLine(string.Format("  {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand));
        }
    }
}
