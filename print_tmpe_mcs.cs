using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string tmpePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var assembly = AssemblyDefinition.ReadAssembly(tmpePath);

        var type = assembly.Modules.SelectMany(m => m.Types).FirstOrDefault(t => t.FullName == "TrafficManager.Manager.Impl.VehicleBehaviorManager");
        foreach (var m in type.Methods.Where(meth => meth.Name == "MayChangeSegment"))
        {
            Console.WriteLine("MayChangeSegment (" + m.Parameters.Count + " params):");
            if (m.HasBody)
            {
                for (int i = 0; i < m.Body.Instructions.Count; i++)
                {
                    var instr = m.Body.Instructions[i];
                    if (instr.Operand != null && instr.Operand.ToString().Contains("MayDespawn"))
                    {
                        for (int j = Math.Max(0, i - 4); j <= Math.Min(m.Body.Instructions.Count - 1, i + 4); j++)
                        {
                            Console.WriteLine("   " + m.Body.Instructions[j].Offset.ToString("X4") + ": " + m.Body.Instructions[j].OpCode + " " + m.Body.Instructions[j].Operand);
                        }
                    }
                }
            }
        }
    }
}
