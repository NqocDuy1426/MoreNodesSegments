using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var assembly = AssemblyDefinition.ReadAssembly(tmpePath);
        var type = assembly.MainModule.GetType("TrafficManager.Manager.Impl.TrafficPriorityManager");
        foreach (var m in type.Methods)
        {
            if (m.Name == ".ctor" || m.Name == "OnLevelLoading")
            {
                Console.WriteLine("=== Method: " + m.FullName + " ===");
                foreach (var inst in m.Body.Instructions)
                {
                    Console.WriteLine("  " + inst.OpCode + " " + inst.Operand);
                }
            }
        }
    }
}
