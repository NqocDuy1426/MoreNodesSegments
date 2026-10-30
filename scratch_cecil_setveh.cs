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

        var t = asm.MainModule.GetType("HumanAI");
        foreach (var m in t.Methods)
        {
            if (m.Name == "SetCurrentVehicle")
            {
                Console.WriteLine("--- METHOD: " + m.FullName + " ---");
                if (m.Body != null)
                {
                    foreach (var inst in m.Body.Instructions)
                    {
                        Console.WriteLine("  {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand);
                    }
                }
            }
        }
    }
}
