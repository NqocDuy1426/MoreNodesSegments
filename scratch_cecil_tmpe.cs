using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        Console.WriteLine("Inspecting " + asm.FullName);
        foreach (var type in asm.MainModule.Types)
        {
            InspectType(type);
        }
    }

    static void InspectType(TypeDefinition type)
    {
        foreach (var m in type.Methods)
        {
            if (!m.HasBody) continue;
            foreach (var inst in m.Body.Instructions)
            {
                if (inst.OpCode == OpCodes.Ldc_I4)
                {
                    int val = (int)inst.Operand;
                    if (val == 36864 || val == 32768 || val == 73728 || val == 36863 || val == 32767 || val == 73727)
                    {
                        Console.WriteLine($"{type.FullName}.{m.Name} -> Ldc_I4 {val}");
                        break;
                    }
                }
            }
        }
        foreach (var nested in type.NestedTypes)
        {
            InspectType(nested);
        }
    }
}
