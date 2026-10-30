using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var gameManaged = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(gameManaged);
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };
        var asm = AssemblyDefinition.ReadAssembly(Path.Combine(gameManaged, "Assembly-CSharp.dll"), readerParams);

        var sdw = asm.MainModule.GetType("SerializableDataWrapper");
        var m = sdw.Methods.FirstOrDefault(x => x.Name == "OnLoadData");
        Console.WriteLine("=== SerializableDataWrapper.OnLoadData ===");
        foreach (var inst in m.Body.Instructions)
        {
            Console.WriteLine($"   {inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
        }
    }
}
