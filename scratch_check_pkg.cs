using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        GAME_DIR="/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(GAME_DIR + "/ColossalManaged.dll");
        foreach (var t in asm.MainModule.Types)
        {
            if (t.FullName.Contains("Package") || t.FullName.Contains("Save"))
            {
                Console.WriteLine(t.FullName);
                foreach (var m in t.Methods)
                {
                    if (m.IsStatic || m.Name.Contains("Load") || m.Name.Contains("Open") || m.Name.Contains("Read"))
                        Console.WriteLine("  " + m.Name);
                }
            }
        }
    }
    static string GAME_DIR;
}
