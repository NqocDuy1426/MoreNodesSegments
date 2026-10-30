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
        var nmd = nm.NestedTypes[0]; // NetManager/Data
        foreach (var m in nmd.Methods)
        {
            if (m.Name == "Deserialize")
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
