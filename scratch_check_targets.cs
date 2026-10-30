using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.GetType("MoreNetworkCapacity.Patches.GeneratedPatchTargets");
        if (type != null)
        {
            var m = type.Methods[0];
            string curClass = "";
            foreach (var ins in m.Body.Instructions)
            {
                if (ins.OpCode == OpCodes.Ldstr)
                {
                    string s = (string)ins.Operand;
                    if (s == "NetManager" || s == "RoadBaseAI" || s == "CarAI")
                    {
                        Console.WriteLine("Target found: " + s);
                    }
                }
            }
        }
    }
}
