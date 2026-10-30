using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

class DumpSimStepCallers
{
    static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(Path.Combine(gameDir, "Assembly-CSharp.dll"));
        foreach (var type in asm.MainModule.Types)
        {
            foreach (var m in type.Methods)
            {
                if (m.HasBody)
                {
                    foreach (var inst in m.Body.Instructions)
                    {
                        if (inst.Operand != null && inst.Operand.ToString().Contains("TrafficLightSimulationStep"))
                        {
                            Console.WriteLine(type.FullName + "." + m.Name + " calls " + inst.Operand);
                        }
                    }
                }
            }
        }
    }
}
