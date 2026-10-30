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
            if (m.Name == "SimulationStep" && m.Parameters.Count == 4)
            {
                for (int i = 0; i < m.Body.Instructions.Count; i++)
                {
                    var inst = m.Body.Instructions[i];
                    if (inst.Offset >= 0x0680 && inst.Offset <= 0x0730)
                    {
                        Console.WriteLine("  {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand);
                    }
                }
            }
        }
    }
}
