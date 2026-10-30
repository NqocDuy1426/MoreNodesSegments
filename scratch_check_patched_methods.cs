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
            if (t.FullName.StartsWith("TrafficManager.Custom.PathFinding.")) continue;
            foreach (var m in t.Methods)
            {
                if (!m.HasBody) continue;
                bool has = false;
                foreach (var inst in m.Body.Instructions)
                {
                    if (inst.OpCode == OpCodes.Ldc_I4)
                    {
                        int v = (int)inst.Operand;
                        if (v == 36864 || v == 32768 || v == 36863 || v == 32767 || v == 73728 || v == 73727)
                        {
                            has = true; break;
                        }
                    }
                }
                if (has)
                {
                    Console.WriteLine($"{t.FullName}.{m.Name}");
                }
            }
        }
    }
}
