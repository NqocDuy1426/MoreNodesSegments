using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var nm = asm.MainModule.GetType("NetManager");
        TypeDefinition data = null;
        foreach (var t in nm.NestedTypes)
        {
            if (t.Name == "Data") { data = t; break; }
        }
        foreach (var m in data.Methods)
        {
            if (m.Name == "Deserialize")
            {
                Console.WriteLine("Instructions: " + m.Body.Instructions.Count);
                for (int i = 0; i < m.Body.Instructions.Count; i++)
                {
                    var inst = m.Body.Instructions[i];
                    if (inst.OpCode.Code == Code.Ldelem_Any ||
                        inst.OpCode.Code == Code.Ldelem_I ||
                        inst.OpCode.Code == Code.Ldelem_I1 ||
                        inst.OpCode.Code == Code.Ldelem_I2 ||
                        inst.OpCode.Code == Code.Ldelem_I4 ||
                        inst.OpCode.Code == Code.Ldelem_I8 ||
                        inst.OpCode.Code == Code.Ldelem_R4 ||
                        inst.OpCode.Code == Code.Ldelem_R8 ||
                        inst.OpCode.Code == Code.Ldelem_Ref ||
                        inst.OpCode.Code == Code.Ldelem_U1 ||
                        inst.OpCode.Code == Code.Ldelem_U2 ||
                        inst.OpCode.Code == Code.Ldelem_U4 ||
                        inst.OpCode.Code == Code.Ldelema ||
                        inst.OpCode.Code == Code.Stelem_Any ||
                        inst.OpCode.Code == Code.Stelem_I ||
                        inst.OpCode.Code == Code.Stelem_I1 ||
                        inst.OpCode.Code == Code.Stelem_I2 ||
                        inst.OpCode.Code == Code.Stelem_I4 ||
                        inst.OpCode.Code == Code.Stelem_I8 ||
                        inst.OpCode.Code == Code.Stelem_R4 ||
                        inst.OpCode.Code == Code.Stelem_R8 ||
                        inst.OpCode.Code == Code.Stelem_Ref)
                    {
                        Console.WriteLine($"Offset 0x{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                        // print 3 instructions before
                        for (int j = Math.Max(0, i - 4); j < i; j++)
                        {
                            Console.WriteLine($"    0x{m.Body.Instructions[j].Offset:X4}: {m.Body.Instructions[j].OpCode} {m.Body.Instructions[j].Operand}");
                        }
                    }
                }
            }
        }
    }
}
