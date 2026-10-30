using System;
using Mono.Cecil;

public class ListCoreTypeNames
{
    public static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("MoreNetworkCapacity.Core.dll");
        foreach (var t in asm.MainModule.Types)
        {
            Console.WriteLine(t.FullName);
        }
    }
}
