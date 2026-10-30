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
        MethodDefinition deser = null;
        foreach (var m in nmd.Methods) if (m.Name == "Deserialize") deser = m;
        for (int i = 0; i < deser.Body.Instructions.Count; i++)
        {
            var inst = deser.Body.Instructions[i];
            if (inst.OpCode == OpCodes.Ldloc_S || inst.OpCode == OpCodes.Ldloc)
            {
                if (inst.Operand is VariableDefinition vd && (vd.Index == 7 || vd.Index == 8))
                {
                    Console.WriteLine(string.Format("Inst {0}: {1} V_{2}", i, inst.OpCode, vd.Index));
                    // print context
                    for (int k = Math.Max(0, i - 3); k <= Math.Min(deser.Body.Instructions.Count - 1, i + 3); k++)
                    {
                        var c = deser.Body.Instructions[k];
                        Console.WriteLine(string.Format("   {0}: {1} {2}", k, c.OpCode, c.Operand));
                    }
                }
            }
        }
    }
}
