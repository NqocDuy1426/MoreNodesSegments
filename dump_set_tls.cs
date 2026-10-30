using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class DumpSetTls
{
    static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(Path.Combine(gameDir, "Assembly-CSharp.dll"));
        var roadBaseAI = asm.MainModule.GetType("RoadBaseAI");
        var m = roadBaseAI.Methods.FirstOrDefault(x => x.Name == "SetTrafficLightState");
        if (m != null)
        {
            Console.WriteLine(m.FullName);
            foreach (var inst in m.Body.Instructions)
            {
                Console.WriteLine(string.Format("  {0,-10} {1,-12} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand));
            }
        }
    }
}
