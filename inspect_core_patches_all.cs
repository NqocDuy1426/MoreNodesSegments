using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/MoreNetworkCapacity.Core.dll");
        foreach (var type in asm.MainModule.Types)
        {
            foreach (var ca in type.CustomAttributes)
            {
                if (ca.AttributeType.Name == "HarmonyPatch")
                {
                    var args = string.Join(", ", ca.ConstructorArguments.Select(a => a.Value?.ToString() ?? "null"));
                    Console.WriteLine($"[Core Patch] {type.FullName} -> {args}");
                }
            }
            foreach (var m in type.Methods)
            {
                foreach (var ca in m.CustomAttributes)
                {
                    if (ca.AttributeType.Name == "HarmonyPatch")
                    {
                        var args = string.Join(", ", ca.ConstructorArguments.Select(a => a.Value?.ToString() ?? "null"));
                        Console.WriteLine($"[Core Patch Method] {type.Name}.{m.Name} -> {args}");
                    }
                }
            }
        }
    }
}
