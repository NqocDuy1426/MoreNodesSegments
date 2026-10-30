using System;
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
                for (int i = 0; i < m.Body.Instructions.Count; i++)
                {
                    var inst = m.Body.Instructions[i];
                    string s = inst.ToString();
                    if (s.Contains("V_7") || s.Contains("V_8") || s.Contains("ldloc.2") || s.Contains("ldloc.3"))
                    {
                        Console.WriteLine($"IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                    }
                }
            }
        }
    }
}
