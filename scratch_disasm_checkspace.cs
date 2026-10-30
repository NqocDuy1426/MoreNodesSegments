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
            if (m.Name.Contains("CheckSpace"))
            {
                Console.WriteLine("--- NetLane." + m.Name + " (" + m.Parameters.Count + " params) ---");
                foreach (var inst in m.Body.Instructions)
                {
                    Console.WriteLine("  {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand);
                }
            }
        }
    }
}
