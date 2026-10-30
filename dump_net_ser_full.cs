using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

class DumpNetSer
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var netMgr = asm.MainModule.Types.FirstOrDefault(t => t.Name == "NetManager");
        var netMgrData = netMgr.NestedTypes.FirstOrDefault(t => t.Name == "Data");
        var ser = netMgrData.Methods.FirstOrDefault(m => m.Name == "Serialize");
        using (var sw = new StreamWriter("net_ser_full.txt"))
        {
            foreach (var inst in ser.Body.Instructions)
            {
                sw.WriteLine(string.Format("{0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand));
            }
        }
        Console.WriteLine("Dumped net_ser_full.txt with " + ser.Body.Instructions.Count + " instructions");
    }
}
