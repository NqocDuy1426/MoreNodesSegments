using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm1 = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Downloads/depot_255710_2829241538003963999 (1)/MoreNetworkCapacity.Core.dll");
        foreach (var t in asm1.MainModule.Types) {
            if (t.Name == "BuildInfo") {
                foreach (var f in t.Fields) {
                    Console.WriteLine("Download Core BuildInfo: " + f.Name + " = " + f.Constant);
                }
            }
        }

        var asm2 = AssemblyDefinition.ReadAssembly("MoreNetworkCapacity.Core.dll");
        foreach (var t in asm2.MainModule.Types) {
            if (t.Name == "BuildInfo") {
                foreach (var f in t.Fields) {
                    Console.WriteLine("Current Core BuildInfo: " + f.Name + " = " + f.Constant);
                }
            }
        }
    }
}
