using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var lp = asm.MainModule.GetType("LoadingProfiler");
        foreach (var m in lp.Methods.Where(x => x.Name.Contains("Deserialize"))) {
            Console.WriteLine(m.FullName);
        }
    }
}

