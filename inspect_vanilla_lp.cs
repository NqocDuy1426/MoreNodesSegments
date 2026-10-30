using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var lp = asm.MainModule.Types.FirstOrDefault(t => t.Name == "LoadingProfiler");
        foreach (var m in lp.Methods) {
            if (m.Name.Contains("Deserialize")) {
                Console.WriteLine("Method: " + m.FullName);
                foreach (var ins in m.Body.Instructions) {
                    Console.WriteLine("  " + ins.ToString());
                }
            }
        }
    }
}
