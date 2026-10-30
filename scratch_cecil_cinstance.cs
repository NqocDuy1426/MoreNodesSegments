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

        var t = asm.MainModule.GetType("CitizenInstance/Flags");
        Console.WriteLine("\n==============================");
        Console.WriteLine("ENUM: " + t.FullName);
        foreach (var f in t.Fields)
        {
            if (f.IsStatic)
            {
                Console.WriteLine("  {0} = {1} (0x{1:X})", f.Name, f.Constant);
            }
        }
    }
}
