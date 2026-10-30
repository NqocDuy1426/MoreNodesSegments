using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var dnp = asm.MainModule.Types.FirstOrDefault(t => t.Name == "DayNightProperties");
        var iter = dnp.NestedTypes.FirstOrDefault(t => t.Name.Contains("InitializeProperties"));
        Console.WriteLine("Iter: " + (iter != null ? iter.FullName : "null"));
        if (iter != null) {
            var moveNext = iter.Methods.FirstOrDefault(m => m.Name == "MoveNext");
            foreach (var ins in moveNext.Body.Instructions) {
                Console.WriteLine("  " + ins.ToString());
            }
        }
    }
}
