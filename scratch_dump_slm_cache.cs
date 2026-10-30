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
            if (m.Body == null) continue;
            for (int i = 0; i < m.Body.Instructions.Count; i++)
            {
                var inst = m.Body.Instructions[i];
                if (inst.OpCode == OpCodes.Ldfld || inst.OpCode == OpCodes.Ldflda)
                {
                    var fr = inst.Operand as FieldReference;
                    if (fr != null && fr.Name == "cachedLaneSpeedLimits_")
                    {
                        Console.WriteLine("Method {0} accesses cachedLaneSpeedLimits_ at {1:X4}", m.Name, inst.Offset);
                    }
                }
            }
        }
    }
}
