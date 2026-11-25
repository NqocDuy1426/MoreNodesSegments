using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        TypeDefinition td = asm.MainModule.GetType("NetManager/Data");
        MethodDefinition md = null;
        foreach (var m in td.Methods) if (m.Name == "Deserialize") md = m;

        // Convert Cecil instructions to pseudo Harmony CodeInstructions
        Console.WriteLine("Cecil instructions count: " + md.Body.Instructions.Count);
        int cutIndex = -1;
        for (int i = 0; i < md.Body.Instructions.Count; i++)
        {
            var inst = md.Body.Instructions[i];
            if (inst.OpCode == Mono.Cecil.Cil.OpCodes.Ldc_I4 && inst.Operand is int val && val == 321)
            {
                // The instruction before is ldarg.1 at IL_1F91
                if (i > 0 && md.Body.Instructions[i - 1].OpCode == Mono.Cecil.Cil.OpCodes.Ldarg_1)
                {
                    cutIndex = i - 1;
                    Console.WriteLine("Found cut point at index " + cutIndex + " (IL_" + md.Body.Instructions[cutIndex].Offset.ToString("X4") + ")");
                    break;
                }
            }
        }

        if (cutIndex != -1)
        {
            Console.WriteLine("Instructions before cut: " + cutIndex);
            Console.WriteLine("Instructions after cut to be replaced: " + (md.Body.Instructions.Count - cutIndex));
            Console.WriteLine("Cut target opcode: " + md.Body.Instructions[cutIndex].OpCode);
        }
    }
}
