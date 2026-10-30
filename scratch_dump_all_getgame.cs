using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var t = asm.MainModule.GetType("TrafficManager.Manager.Impl.SpeedLimitManager");
        foreach (var m in t.Methods)
        {
            if (m.Name == "GetGameSpeedLimit")
            {
                Console.WriteLine("========================================");
                Console.WriteLine(m.FullName);
                foreach (var inst in m.Body.Instructions)
                {
                    Console.WriteLine("  {0:X4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand);
                }
            }
        }
    }
}
