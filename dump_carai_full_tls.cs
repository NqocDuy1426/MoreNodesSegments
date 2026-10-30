using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class DumpCarAIFullTls
{
    static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(Path.Combine(gameDir, "Assembly-CSharp.dll"));
        var carAI = asm.MainModule.GetType("CarAI");
        var m = carAI.Methods.FirstOrDefault(x => x.Name == "CalculateSegmentPosition" && x.Parameters.Count == 13);
        if (m != null)
        {
            Console.WriteLine(m.FullName);
            bool inTls = false;
            for (int i = 0; i < m.Body.Instructions.Count; i++)
            {
                var inst = m.Body.Instructions[i];
                if (inst.Offset >= 0x0460 && inst.Offset <= 0x0620)
                {
                    Console.WriteLine(string.Format("  {0,-10} {1,-12} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand));
                }
            }
        }
    }
}
