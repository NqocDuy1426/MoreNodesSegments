using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.GetType("RoadBaseAI");
        MethodDefinition target = null;
        foreach (var m in type.Methods)
        {
            if (m.Name == "TrafficLightSimulationStep") target = m;
        }

        Console.WriteLine("Target: " + target.FullName);
        foreach (var ins in target.Body.Instructions)
        {
            string op = ins.OpCode.ToString();
            string operand = ins.Operand != null ? ins.Operand.ToString() : "";
            Console.WriteLine($"{ins.Offset:X4}: {op,-12} {operand}");
        }
    }
}
