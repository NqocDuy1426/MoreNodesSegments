using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var t = asm.MainModule.GetType("NetLane");
        foreach (var m in t.Methods)
        {
            if (m.Name.Contains("Reserve"))
            {
                Console.WriteLine("NetLane." + m.Name);
            }
        }
    }
}
