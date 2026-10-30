using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var gameDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");

        var type = asm.MainModule.GetType("RenderManager");
        foreach (var f in type.Fields)
        {
            if (f.IsStatic && (f.Name.Contains("COLORMAP") || f.Name.Contains("MAP") || f.Name.Contains("OBJECT")))
            {
                Console.WriteLine($"{f.Name} = {f.Constant}");
            }
        }
    }
}
