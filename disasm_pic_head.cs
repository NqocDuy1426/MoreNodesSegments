using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var type = asm.MainModule.GetType("TrafficManager.Custom.PathFinding.CustomPathFind");
        foreach (var m in type.Methods)
        {
            if (m.Name == "ProcessItemCosts" && m.Parameters.Count == 20)
            {
                foreach (var inst in m.Body.Instructions)
                {
                    if (inst.Offset < 0x0045)
                    {
                        Console.WriteLine(string.Format("IL_{0:x4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand));
                    }
                }
            }
        }
    }
}
