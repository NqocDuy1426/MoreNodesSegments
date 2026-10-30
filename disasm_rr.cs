using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var rm = asm.MainModule.GetType("TrafficManager.Manager.Impl.RoutingManager");
        foreach (var m in rm.Methods)
        {
            if (m.Name.Contains("RequestRecalculation") || m.Name.Contains("SimulationStep"))
            {
                Console.WriteLine("========================================");
                Console.WriteLine("METHOD: " + m.FullName);
                Console.WriteLine("========================================");
                if (m.Body != null)
                {
                    foreach (var inst in m.Body.Instructions)
                    {
                        Console.WriteLine(string.Format("IL_{0:x4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand));
                    }
                }
            }
        }
    }
}
