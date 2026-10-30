using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(tmpePath);
        var t = asm.MainModule.GetType("TrafficManager.Patch._VehicleAI._CarAI.CalculateSegmentPositionPatch2");
        var m = t.Methods[1]; // Prefix
        for (int i = 0; i < m.Body.Instructions.Count; i++)
        {
            var ins = m.Body.Instructions[i];
            Console.WriteLine("  {0}: {1} {2}", ins.Offset.ToString("X4"), ins.OpCode, ins.Operand);
        }
    }
}
