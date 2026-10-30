using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.GetType("CarAI");
        foreach (var m in type.Methods)
        {
            if (m.Name == "CalculateSegmentPosition")
            {
                Console.WriteLine("=== CarAI.CalculateSegmentPosition ===");
                bool nearCall = false;
                int count = 0;
                for (int i = 0; i < m.Body.Instructions.Count; i++)
                {
                    var ins = m.Body.Instructions[i];
                    if (ins.ToString().Contains("GetTrafficLightState"))
                    {
                        for (int k = Math.Max(0, i - 15); k <= Math.Min(m.Body.Instructions.Count - 1, i + 25); k++)
                        {
                            Console.WriteLine("  " + m.Body.Instructions[k].ToString());
                        }
                    }
                }
            }
        }
    }
}
