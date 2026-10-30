using System;
using System.Linq;
using Mono.Cecil;

class PrintEncodedArray
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("ColossalManaged.dll");
        var ea = asm.MainModule.Types.FirstOrDefault(t => t.FullName == "ColossalFramework.IO.EncodedArray");
        Console.WriteLine("EncodedArray nested types:");
        foreach (var nested in ea.NestedTypes)
        {
            Console.WriteLine("  " + nested.Name);
            foreach (var m in nested.Methods)
            {
                if (m.Name == "BeginRead" || m.Name == "BeginWrite" || m.Name == "Read" || m.Name == "Write")
                {
                    Console.WriteLine("    " + m.Name + ": " + m.Parameters.Count + " params");
                }
            }
        }
    }
}
