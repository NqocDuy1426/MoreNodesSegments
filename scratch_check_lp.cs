using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var lp = asm.MainModule.Types.FirstOrDefault(t => t.Name == "LoadingProfiler");
        if (lp != null) {
            foreach (var m in lp.Methods) {
                Console.WriteLine("LoadingProfiler method: " + m.FullName);
            }
        }
    }
}
