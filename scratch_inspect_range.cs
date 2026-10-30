using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var lm = asm.MainModule.GetType("LoadingManager");
        var t = lm.NestedTypes.FirstOrDefault(x => x.Name.Contains("LoadLevelCoroutine"));
        var mn = t.Methods.First(m => m.Name == "MoveNext");
        bool print = false;
        for (int i = 0; i < mn.Body.Instructions.Count; i++) {
            var inst = mn.Body.Instructions[i];
            if (inst.Offset == 0x19bb) print = true;
            if (print) {
                Console.WriteLine(inst);
                if (inst.Offset >= 0x1bd7) break;
            }
        }
    }
}

