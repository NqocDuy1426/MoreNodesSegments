using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var gameManaged = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(gameManaged);
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };
        var asm = AssemblyDefinition.ReadAssembly(Path.Combine(gameManaged, "Assembly-CSharp.dll"), readerParams);

        var lm = asm.MainModule.GetType("LoadingManager");
        Check(lm);
        foreach (var n in lm.NestedTypes) Check(n);
    }

    static void Check(TypeDefinition t)
    {
        foreach (var m in t.Methods)
        {
            if (!m.HasBody) continue;
            foreach (var inst in m.Body.Instructions)
            {
                if (inst.Operand != null && inst.Operand.ToString().ToLower().Contains("simulationdataloaded"))
                {
                    Console.WriteLine($"{t.Name}.{m.Name} uses {inst.Operand}");
                }
            }
        }
    }
}
