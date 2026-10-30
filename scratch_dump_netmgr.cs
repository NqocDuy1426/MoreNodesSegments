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
        var type = asm.MainModule.GetType("NetManager");
        foreach (var m in type.Methods)
        {
            if (m.Name == "SimulationStepImpl")
            {
                using (var sw = new StreamWriter("netmgr_simstep.txt"))
                {
                    foreach (var ins in m.Body.Instructions)
                    {
                        sw.WriteLine(ins.ToString());
                    }
                }
                Console.WriteLine("Dumped SimulationStepImpl, instructions: " + m.Body.Instructions.Count);
            }
        }
    }
}
