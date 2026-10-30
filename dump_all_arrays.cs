using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var m = asm.MainModule.GetType("NetManager/Data").Methods.First(x => x.Name == "Deserialize");
        for (int i = 0; i < m.Body.Instructions.Count; i++) {
            var instr = m.Body.Instructions[i];
            if (instr.OpCode.Code == Code.Ldelem_I1 || instr.OpCode.Code == Code.Ldelem_I2 ||
                instr.OpCode.Code == Code.Ldelem_I4 || instr.OpCode.Code == Code.Ldelem_I8 ||
                instr.OpCode.Code == Code.Ldelem_U1 || instr.OpCode.Code == Code.Ldelem_U2 ||
                instr.OpCode.Code == Code.Ldelem_U4 || instr.OpCode.Code == Code.Ldelem_R4 ||
                instr.OpCode.Code == Code.Ldelem_R8 || instr.OpCode.Code == Code.Ldelem_Ref ||
                instr.OpCode.Code == Code.Ldelem_Any || instr.OpCode.Code == Code.Ldelema ||
                instr.OpCode.Code == Code.Stelem_I1 || instr.OpCode.Code == Code.Stelem_I2 ||
                instr.OpCode.Code == Code.Stelem_I4 || instr.OpCode.Code == Code.Stelem_I8 ||
                instr.OpCode.Code == Code.Stelem_R4 || instr.OpCode.Code == Code.Stelem_R8 ||
                instr.OpCode.Code == Code.Stelem_Ref || instr.OpCode.Code == Code.Stelem_Any) {
                
                // Print surrounding context
                int start = Math.Max(0, i - 6);
                Console.WriteLine($"Array op at IL_{instr.Offset:X4} ({instr.OpCode}):");
                for (int j = start; j <= i; j++) {
                    Console.WriteLine($"    IL_{m.Body.Instructions[j].Offset:X4}: {m.Body.Instructions[j].OpCode} {m.Body.Instructions[j].Operand}");
                }
            }
        }
    }
}
