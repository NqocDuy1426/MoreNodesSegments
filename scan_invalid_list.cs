using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

public class ScanInvalidList
{
    public static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        foreach (var t in asm.MainModule.Types)
        {
            CheckType(t);
            foreach (var nested in t.NestedTypes)
            {
                CheckType(nested);
            }
        }
    }

    static void CheckType(TypeDefinition t)
    {
        foreach (var m in t.Methods)
        {
            if (!m.HasBody) continue;
            foreach (var instr in m.Body.Instructions)
            {
                if (instr.OpCode == OpCodes.Ldstr && instr.Operand != null && instr.Operand.ToString().Contains("Invalid list detected"))
                {
                    Console.WriteLine(string.Format("{0}::{1}", t.FullName, m.Name));
                }
            }
        }
    }
}
