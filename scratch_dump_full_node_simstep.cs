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
        foreach (var m in type.Methods)
        {
            if (m.Name == "SimulationStep" && m.Parameters.Count == 2 && m.Parameters[1].ParameterType.Name == "NetNode&")
            {
                using (var sw = new StreamWriter("node_simstep_disasm.txt"))
                {
                    foreach (var ins in m.Body.Instructions)
                    {
                        sw.WriteLine(ins.ToString());
                    }
                }
                Console.WriteLine("Dumped RoadBaseAI.SimulationStep(ushort, ref NetNode), instructions: " + m.Body.Instructions.Count);
            }
        }
    }
}
