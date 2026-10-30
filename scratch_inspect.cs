using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        foreach (var type in asm.MainModule.Types) {
            if (type.Name.StartsWith("LoadingManager")) {
                foreach (var m in type.Methods) {
                    if (m.Name.Contains("LoadLevel") || m.Name.Contains("Essential")) {
                        Console.WriteLine("LoadingManager method: " + m.FullName);
                    }
                }
            }
        }
    }
}
