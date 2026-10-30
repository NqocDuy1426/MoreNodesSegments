using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var gameDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");

        var t = asm.MainModule.GetType("NetManager");
        foreach (var m in t.Methods)
        {
            if (m.Name == "SimulationStepImpl")
            {
                Console.WriteLine("=== NetManager.SimulationStepImpl ===");
                // Look for segment simulation step calls
                foreach (var inst in m.Body.Instructions)
                {
                    if (inst.OpCode == OpCodes.Callvirt || inst.OpCode == OpCodes.Call)
                    {
                        var mr = inst.Operand as MethodReference;
                        if (mr != null && mr.Name == "SimulationStep")
                        {
                            Console.WriteLine($"  IL_{inst.Offset:X4}: calls {mr.DeclaringType.Name}.{mr.Name}");
                        }
                    }
                }
            }
        }
    }
}
