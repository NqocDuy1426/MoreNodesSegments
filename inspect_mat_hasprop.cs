using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("UnityEngine.dll");
        var mat = asm.MainModule.Types.FirstOrDefault(t => t.Name == "Material");
        foreach (var m in mat.Methods) {
            if (m.Name.Contains("HasProperty")) {
                Console.WriteLine("Method: " + m.FullName);
                foreach (var p in m.Parameters) {
                    Console.WriteLine("  param: " + p.ParameterType.FullName);
                }
            }
        }
    }
}
