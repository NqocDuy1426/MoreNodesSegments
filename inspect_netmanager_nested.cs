using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string gameDir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var assembly = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");
        var nm = assembly.MainModule.GetType("NetManager");
        foreach (var nested in nm.NestedTypes)
        {
            Console.WriteLine("Nested: " + nested.FullName);
            foreach (var m in nested.Methods)
            {
                Console.WriteLine("  " + m.FullName);
            }
        }
    }
}
