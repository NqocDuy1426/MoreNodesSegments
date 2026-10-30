using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string gameDir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var readerParams = new ReaderParameters { ReadSymbols = false };
        var asm = AssemblyDefinition.ReadAssembly(Path.Combine(gameDir, "Assembly-CSharp.dll"), readerParams);

        foreach (var t in asm.MainModule.Types)
        {
            foreach (var m in t.Methods)
            {
                if (m.HasBody)
                {
                    foreach (var ins in m.Body.Instructions)
                    {
                        if (ins.OpCode == OpCodes.Callvirt || ins.OpCode == OpCodes.Call)
                        {
                            var operand = ins.Operand.ToString();
                            if (operand.Contains("TrafficLightSimulationStep"))
                            {
                                Console.WriteLine("Caller: " + t.FullName + "::" + m.Name);
                            }
                        }
                    }
                }
            }
        }
    }
}
