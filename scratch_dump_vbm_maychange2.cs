using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var t = asm.MainModule.GetType("TrafficManager.Manager.Impl.VehicleBehaviorManager");
        foreach (var m in t.Methods)
        {
            if (m.Name.Contains("MayChangeSegment"))
            {
                Console.WriteLine("========================================");
                Console.WriteLine(m.FullName);
                foreach (var inst in m.Body.Instructions)
                {
                    if (inst.OpCode == OpCodes.Ldfld || inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt)
                    {
                        var mr = inst.Operand as MethodReference;
                        var fr = inst.Operand as FieldReference;
                        if (mr != null && (mr.Name.Contains("Speed") || mr.Name.Contains("Transit")))
                            Console.WriteLine("  Calls {0}", mr.Name);
                        if (fr != null && (fr.Name.Contains("speed") || fr.Name.Contains("Transit")))
                            Console.WriteLine("  Accesses {0}", fr.Name);
                    }
                }
            }
        }
    }
}
