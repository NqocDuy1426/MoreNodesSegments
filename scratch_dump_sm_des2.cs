using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var sm = asm.MainModule.GetType("SimulationManager");
        TypeDefinition data = null;
        foreach (var t in sm.NestedTypes)
        {
            if (t.Name == "Data") { data = t; break; }
        }
        foreach (var m in data.Methods)
        {
            if (m.Name == "Deserialize")
            {
                foreach (var inst in m.Body.Instructions)
                {
                    if (inst.OpCode == OpCodes.Callvirt || inst.OpCode == OpCodes.Call)
                    {
                        Console.WriteLine($"IL_{inst.Offset:X4}: {inst.Operand}");
                    }
                }
            }
        }
    }
}
