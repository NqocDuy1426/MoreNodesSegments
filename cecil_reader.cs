using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string dll = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/ColossalManaged.dll";
        var mod = ModuleDefinition.ReadModule(dll);
        var t = mod.GetType("ColossalFramework.Packaging.PackageReader");
        Console.WriteLine("Type: " + t.FullName);
        foreach (var m in t.Methods)
        {
            Console.WriteLine("  Method: " + m.FullName);
        }
    }
}
