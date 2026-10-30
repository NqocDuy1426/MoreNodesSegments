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

        var vm = asm.MainModule.GetType("VehicleManager");
        foreach (var nested in vm.NestedTypes)
        {
            if (nested.Name == "Data")
            {
                foreach (var m in nested.Methods)
                {
                    if (m.Name == "Deserialize")
                    {
                        Console.WriteLine("VehicleManager.Data.Deserialize (" + m.Parameters.Count + " params)");
                        foreach (var inst in m.Body.Instructions)
                        {
                            Console.WriteLine("  {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand);
                        }
                    }
                }
            }
        }
    }
}
