using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string p = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(p);

        var type = asm.MainModule.GetType("TrafficManager.UI.SubTools.LaneArrows.LaneArrowTool");
        foreach (var m in type.Methods)
        {
            Console.WriteLine($"=== {m.Name} ===");
            foreach (var inst in m.Body.Instructions)
            {
                if (inst.OpCode == OpCodes.Ldc_I4 || inst.OpCode == OpCodes.Ldc_I4_S)
                {
                    int v = Convert.ToInt32(inst.Operand);
                    if (v > 1000) Console.WriteLine($"  const: {v}");
                }
            }
        }
    }
}
