using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Frameworks/MonoEmbedRuntime/osx");
        resolver.AddSearchDirectory("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed");

        var p = new ReaderParameters { AssemblyResolver = resolver };
        var ass = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll", p);

        var sm = ass.MainModule.GetType("SimulationManager");
        foreach (var m in sm.Methods)
        {
            if (m.Name == "SimulationStep" || m.Name == "get_FinalSimulationSpeed" || m.Name == "FixedUpdate" || m.Name == "SimulationThread")
            {
                Console.WriteLine("================ METHOD: " + m.Name + " ================");
                if (m.HasBody)
                {
                    foreach (var ins in m.Body.Instructions)
                    {
                        Console.WriteLine(string.Format("  IL_{0:X4}: {1} {2}", ins.Offset, ins.OpCode, ins.Operand));
                    }
                }
            }
        }
    }
}
