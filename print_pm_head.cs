using System;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(gameDir);
        var p = new ReaderParameters { AssemblyResolver = resolver };
        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll", p);
        var pmData = asm.MainModule.GetType("PathManager/Data");
        var des = pmData.Methods.First(m => m.Name == "Deserialize");
        for (int i = 0; i < 60; i++)
        {
            Console.WriteLine(des.Body.Instructions[i]);
        }
    }
}
