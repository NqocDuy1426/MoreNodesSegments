using System;
using System.Linq;
using Mono.Cecil;

class PrintNetSer
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var netMgr = asm.MainModule.Types.FirstOrDefault(t => t.Name == "NetManager");
        var netMgrData = netMgr.NestedTypes.FirstOrDefault(t => t.Name == "Data");
        var ser = netMgrData.Methods.FirstOrDefault(m => m.Name == "Serialize");
        Console.WriteLine("NetManager.Data.Serialize instructions: " + ser.Body.Instructions.Count);
        for (int i = 0; i < Math.Min(100, ser.Body.Instructions.Count); i++)
        {
            var inst = ser.Body.Instructions[i];
            Console.WriteLine(string.Format("  {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand));
        }
    }
}
