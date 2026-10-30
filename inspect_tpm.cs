using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var assembly = AssemblyDefinition.ReadAssembly(tmpePath);
        foreach (var type in assembly.MainModule.Types)
        {
            if (type.FullName == "TrafficManager.Manager.Impl.TrafficPriorityManager" ||
                type.FullName == "TrafficManager.UI.SubTools.PrioritySigns.PrioritySignsTool")
            {
                Console.WriteLine("Type: " + type.FullName);
                foreach (var m in type.Methods)
                {
                    if (m.Name == "HasNodePrioritySign" || m.Name == "RefreshCurrentPriorityNodeIds")
                    {
                        Console.WriteLine("Method: " + m.FullName);
                        foreach (var inst in m.Body.Instructions)
                        {
                            Console.WriteLine("  " + inst.OpCode + " " + inst.Operand);
                        }
                    }
                }
            }
        }
    }
}
