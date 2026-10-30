using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var gameDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");

        foreach (var t in asm.MainModule.Types)
        {
            if (t.Name.IndexOf("traffic", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Console.WriteLine($"Type: {t.Name}");
                foreach (var m in t.Methods)
                {
                    if (m.IsPublic && !m.IsGetter && !m.IsSetter)
                        Console.WriteLine($"  {m.Name}");
                }
            }
        }
    }
}
