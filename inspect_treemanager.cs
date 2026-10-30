using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var tm = asm.MainModule.Types.FirstOrDefault(t => t.Name == "TreeManager");
        Console.WriteLine("TreeManager found: " + (tm != null));
        if (tm != null) {
            foreach (var m in tm.Methods) {
                if (m.Name.Contains("UpdateData") || m.Name.Contains("LateUpdateData") || m.Name.Contains("EarlyUpdateData")) {
                    Console.WriteLine("TM method: " + m.FullName);
                    if (m.HasBody) {
                        foreach (var ins in m.Body.Instructions) {
                            Console.WriteLine("  " + ins.ToString());
                        }
                    }
                }
            }
        }
    }
}
