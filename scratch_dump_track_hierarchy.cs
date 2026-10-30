using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var gameDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");

        var t = asm.MainModule.GetType("TrainTrackAI");
        while (t != null)
        {
            Console.WriteLine(t.FullName);
            t = t.BaseType != null ? t.BaseType.Resolve() : null;
        }
    }
}
