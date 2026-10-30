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

        var type = asm.MainModule.GetType("TrafficManager.UI.TrafficManagerTool");
        foreach (var m in type.Methods)
        {
            if (m.Body == null) continue;
            foreach (var inst in m.Body.Instructions)
            {
                if (inst.OpCode == OpCodes.Ldc_I4 || inst.OpCode == OpCodes.Ldc_I4_S)
                {
                    int v = Convert.ToInt32(inst.Operand);
                    if (v >= 30000)
                    {
                        Console.WriteLine($"TrafficManagerTool::{m.Name} has const {v}");
                    }
                }
            }
        }
    }
}
