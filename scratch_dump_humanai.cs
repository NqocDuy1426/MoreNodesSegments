using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.GetType("HumanAI");
        foreach (var m in type.Methods)
        {
            if (m.Name == "CheckTrafficLights")
            {
                Console.WriteLine("=== HumanAI.CheckTrafficLights ===");
                for (int i = 0; i < m.Body.Instructions.Count; i++)
                {
                    var ins = m.Body.Instructions[i];
                    if (ins.ToString().Contains("32768") || ins.ToString().Contains("GetTrafficLightState"))
                    {
                        for (int k = Math.Max(0, i - 5); k <= Math.Min(m.Body.Instructions.Count - 1, i + 5); k++)
                        {
                            Console.WriteLine("  " + m.Body.Instructions[k].ToString());
                        }
                    }
                }
            }
        }
    }
}
