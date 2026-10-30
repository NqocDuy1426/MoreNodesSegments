using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        foreach (var t in asm.MainModule.Types)
        {
            foreach (var m in t.Methods)
            {
                if (!m.HasBody) continue;
                foreach (var inst in m.Body.Instructions)
                {
                    if (inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt)
                    {
                        var mr = inst.Operand as MethodReference;
                        if (mr != null && mr.DeclaringType.Name == "SegmentEndManager" && mr.Name == "GetSegmentEnd")
                        {
                            Console.WriteLine($"{t.FullName}.{m.Name}");
                            break;
                        }
                    }
                }
            }
        }
    }
}
