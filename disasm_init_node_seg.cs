using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string gameDir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var assembly = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");
        var nm = assembly.MainModule.GetType("NetManager");
        foreach (var m in nm.Methods)
        {
            if (m.Name == "InitializeNode" || m.Name == "InitializeSegment")
            {
                Console.WriteLine("=== Method: " + m.FullName + " ===");
                foreach (var inst in m.Body.Instructions)
                {
                    Console.WriteLine("  " + inst.OpCode + " " + inst.Operand);
                }
            }
        }
    }
}
