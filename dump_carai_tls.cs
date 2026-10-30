using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class DumpCarAITls
{
    static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(Path.Combine(gameDir, "Assembly-CSharp.dll"));
        var carAI = asm.MainModule.GetType("CarAI");
        foreach (var m in carAI.Methods.Where(x => x.Name == "CalculateSegmentPosition"))
        {
            Console.WriteLine("METHOD: " + m.FullName + " (params: " + m.Parameters.Count + ")");
            for (int i = 0; i < m.Body.Instructions.Count; i++)
            {
                var inst = m.Body.Instructions[i];
                if (inst.Operand != null && inst.Operand.ToString().Contains("GetTrafficLightState"))
                {
                    int start = Math.Max(0, i - 15);
                    int end = Math.Min(m.Body.Instructions.Count - 1, i + 25);
                    for (int j = start; j <= end; j++)
                    {
                        Console.WriteLine(string.Format("  {0,-10} {1,-12} {2}", m.Body.Instructions[j].Offset.ToString("X4"), m.Body.Instructions[j].OpCode, m.Body.Instructions[j].Operand));
                    }
                }
            }
        }
    }
}
