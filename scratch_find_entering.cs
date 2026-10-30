using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed");
        var readerParams = new ReaderParameters { AssemblyResolver = resolver };
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll", readerParams);

        foreach (var t in asm.MainModule.Types)
        {
            foreach (var m in t.Methods)
            {
                if (m.Body == null) continue;
                foreach (var inst in m.Body.Instructions)
                {
                    if (inst.OpCode == OpCodes.Ldc_I4 && (int)inst.Operand == 2048)
                    {
                        Console.WriteLine("Type: {0}, Method: {1}", t.FullName, m.FullName);
                        break;
                    }
                }
            }
        }
    }
}
