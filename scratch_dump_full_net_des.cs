using System;
using System.IO;
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
                using (var sw = new StreamWriter("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/netmanager_deserialize_full.cil"))
                {
                    for (int i = 0; i < m.Body.Instructions.Count; i++)
                    {
                        var inst = m.Body.Instructions[i];
                        sw.WriteLine($"IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                    }
                }
                Console.WriteLine("Written " + m.Body.Instructions.Count + " instructions.");
            }
        }
    }
}
