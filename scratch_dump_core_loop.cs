using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.GetType("MoreNetworkCapacity.Patches.CoreLoopPatch");
        Console.WriteLine("Type found: " + (type != null));
        if (type != null)
        {
            foreach (var ca in type.CustomAttributes)
            {
                Console.WriteLine("Attribute: " + ca.AttributeType.FullName);
                foreach (var arg in ca.ConstructorArguments)
                {
                    Console.WriteLine("  Arg: " + arg.Value);
                }
            }
            foreach (var m in type.Methods)
            {
                Console.WriteLine("Method: " + m.FullName);
                if (m.HasBody)
                {
                    foreach (var ins in m.Body.Instructions)
                    {
                        Console.WriteLine("  " + ins.ToString());
                    }
                }
            }
        }
    }
}
