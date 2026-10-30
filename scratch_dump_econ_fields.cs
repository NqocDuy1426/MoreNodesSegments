using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var gameDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");

        var type = asm.MainModule.GetType("EconomyManager");
        foreach (var f in type.Fields)
        {
            if (f.FieldType.Name.Contains("[]") || f.FieldType.Name.Contains("Int64"))
            {
                Console.WriteLine($"  {f.FieldType.Name} {f.Name}");
            }
        }
    }
}
