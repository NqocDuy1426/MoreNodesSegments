using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        foreach (var t in asm.MainModule.Types)
        {
            CheckType(t);
        }
    }

    static void CheckType(TypeDefinition t)
    {
        foreach (var m in t.Methods)
        {
            if (!m.HasBody) continue;
            foreach (var inst in m.Body.Instructions)
            {
                if (inst.Operand is MethodReference mr && mr.Name == "GetIndex" && mr.DeclaringType.Name == "SegmentEndManager")
                {
                    Console.WriteLine($"{t.FullName}.{m.Name} calls SegmentEndManager.GetIndex");
                    break;
                }
            }
        }
        foreach (var nested in t.NestedTypes)
        {
            CheckType(nested);
        }
    }
}
