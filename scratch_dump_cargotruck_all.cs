using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed");
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll", readerParams);

        var t = asm.MainModule.GetType("CargoTruckAI");
        Console.WriteLine("BaseType: " + t.BaseType.FullName);
        foreach (var m in t.Methods)
        {
            if (m.Name.Contains("SimulationStep"))
                Console.WriteLine("Method: " + m.FullName);
        }
    }
}
