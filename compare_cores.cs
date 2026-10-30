using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm1 = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Downloads/depot_255710_2829241538003963999 (1)/MoreNetworkCapacity.Core.dll");
        Console.WriteLine("Core from Download:");
        Console.WriteLine("  Assembly: " + asm1.FullName);
        foreach (var t in asm1.MainModule.Types) {
            Console.WriteLine("  Type: " + t.FullName);
        }

        var asm2 = AssemblyDefinition.ReadAssembly("MoreNetworkCapacity.Core.dll");
        Console.WriteLine("\nCore in Workspace:");
        Console.WriteLine("  Assembly: " + asm2.FullName);
        foreach (var t in asm2.MainModule.Types) {
            Console.WriteLine("  Type: " + t.FullName);
        }
    }
}
