using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        string[] types = { "TrafficManager.Manager.Impl.SegmentEndManager", "TrafficManager.Manager.Impl.ExtSegmentEndManager" };
        foreach (var tName in types)
        {
            var t = asm.MainModule.GetType(tName);
            Console.WriteLine("=== " + t.FullName + " ===");
            foreach (var m in t.Methods)
            {
                if (m.Name.Contains("GetIndex") || m.Name.Contains("GetSegmentAndNode"))
                {
                    Console.WriteLine("  " + m.FullName);
                    foreach (var inst in m.Body.Instructions)
                    {
                        Console.WriteLine("    {0:X4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand);
                    }
                }
            }
        }
    }
}
