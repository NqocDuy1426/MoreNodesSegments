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
        MethodDefinition ser = null;
        foreach (var m in nmd.Methods) if (m.Name == "Serialize") ser = m;
        for (int i = 0; i < 40; i++)
        {
            var inst = ser.Body.Instructions[i];
            Console.WriteLine(string.Format("{0}: {1} {2}", i, inst.OpCode, inst.Operand));
        }
    }
}
