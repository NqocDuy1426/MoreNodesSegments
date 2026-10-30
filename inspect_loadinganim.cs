using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var la = asm.MainModule.Types.FirstOrDefault(t => t.Name == "LoadingAnimation");
        Console.WriteLine("LoadingAnimation: " + (la != null ? la.FullName : "null"));
        if (la != null) {
            foreach (var m in la.Methods) {
                if (m.Name.Contains("LoadingProgress") || m.Name.Contains("Update")) {
                    Console.WriteLine("Method: " + m.FullName);
                    if (m.HasBody) {
                        foreach (var ins in m.Body.Instructions) {
                            if (ins.Operand != null) {
                                Console.WriteLine("  " + ins.ToString());
                            }
                        }
                    }
                }
            }
        }
    }
}
