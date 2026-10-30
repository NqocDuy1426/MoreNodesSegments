using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string gameDir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var assembly = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");
        var lm = assembly.MainModule.GetType("LoadingManager");
        foreach (var nested in lm.NestedTypes)
        {
            Console.WriteLine(nested.Name);
        }
    }
}
