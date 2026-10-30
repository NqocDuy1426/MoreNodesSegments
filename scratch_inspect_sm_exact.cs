using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll");
        var sm = asm.MainModule.GetType("SimulationManager");
        var data = sm.NestedTypes.First(t => t.Name == "Data");
        var des = data.Methods.First(m => m.Name == "Deserialize");
        for (int i = 0; i < des.Body.Instructions.Count; i++)
        {
            var inst = des.Body.Instructions[i];
            if (inst.Offset >= 0x0180 && inst.Offset <= 0x0280)
                Console.WriteLine(string.Format("IL_{0:X4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand));
        }
    }
}
