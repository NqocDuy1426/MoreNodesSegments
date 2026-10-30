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
            bool uses = false;
            foreach (var inst in m.Body.Instructions)
            {
                var fr = inst.Operand as FieldReference;
                if (fr != null && fr.Name == "cachedLaneSpeedLimits_")
                {
                    uses = true;
                    break;
                }
            }
            if (uses)
            {
                Console.WriteLine($"\n=== SpeedLimitManager.{m.Name} ===");
                foreach (var inst in m.Body.Instructions)
                {
                    Console.WriteLine($"  IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                }
            }
        }
    }
}
