using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var assembly = AssemblyDefinition.ReadAssembly(tmpePath);
        var type = assembly.MainModule.Types.FirstOrDefault(t => t.FullName == "TrafficManager.Custom.PathFinding.CustomPathFind");

        var m = type.Methods.FirstOrDefault(x => x.Name == "ProcessItemCosts" && x.Parameters.Count == 20);
        for (int i = 0; i < m.Body.Instructions.Count; i++)
        {
            var inst = m.Body.Instructions[i];
            if (inst.OpCode == OpCodes.Callvirt || inst.OpCode == OpCodes.Ldfld || inst.OpCode == OpCodes.Ldelem_Ref)
            {
                // look at previous 3 instructions
                string prev = "";
                for (int j = Math.Max(0, i - 2); j < i; j++)
                {
                    prev += m.Body.Instructions[j].OpCode + " " + m.Body.Instructions[j].Operand + " -> ";
                }
                Console.WriteLine(string.Format("IL_{0:X4}: {1} {2}  [Prev: {3}]", inst.Offset, inst.OpCode, inst.Operand, prev));
            }
        }
    }
}
