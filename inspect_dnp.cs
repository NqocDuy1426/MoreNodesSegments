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

        var dnp = asm.MainModule.GetType("DayNightProperties");
        foreach (var nested in dnp.NestedTypes)
        {
            Console.WriteLine("Nested: " + nested.FullName);
            var moveNext = nested.Methods.FirstOrDefault(m => m.Name == "MoveNext");
            if (moveNext != null)
            {
                foreach (var inst in moveNext.Body.Instructions)
                {
                    Console.WriteLine($"   {inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                }
            }
        }
    }
}
