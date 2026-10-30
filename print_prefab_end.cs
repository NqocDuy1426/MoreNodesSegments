using System;
using System.Linq;
using Mono.Cecil;

class PrintPrefabEnd
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var pc = asm.MainModule.Types.FirstOrDefault(t => t.Name == "PrefabCollection`1");
        foreach (var m in pc.Methods.Where(m => m.Name.Contains("EndDeserialize")))
        {
            Console.WriteLine("Method: " + m.Name);
            foreach (var inst in m.Body.Instructions)
            {
                Console.WriteLine(string.Format("  {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand));
            }
        }
    }
}
