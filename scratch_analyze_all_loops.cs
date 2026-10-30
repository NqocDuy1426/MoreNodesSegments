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
                var insts = m.Body.Instructions;
                for (int i = 0; i < insts.Count; i++)
                {
                    var inst = insts[i];
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
                        // Print the array operand (typically pushed before index)
                        // And the index operand
                        string arr = (i >= 2) ? insts[i-2].ToString() : "";
                        string idx = (i >= 1) ? insts[i-1].ToString() : "";
                        Console.WriteLine($"IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand} | Idx: {idx} | Arr: {arr}");
                    }
                }
            }
        }
    }
}
