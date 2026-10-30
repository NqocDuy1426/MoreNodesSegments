using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var t = asm.MainModule.GetType("NetLane");
        foreach (var m in t.Methods)
        {
            if (m.Name == "ReserveSpace" && m.Parameters.Count == 2)
            {
                Console.WriteLine("--- NetLane.ReserveSpace (2 params) ---");
                foreach (var inst in m.Body.Instructions)
                {
                    Console.WriteLine("  {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand);
                }
            }
        }
    }
}
