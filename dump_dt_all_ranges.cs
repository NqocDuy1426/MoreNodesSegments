using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main(string[] args) {
        string mscorlibPath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/mscorlib.dll";
        var asm = AssemblyDefinition.ReadAssembly(mscorlibPath);
        var dt = asm.MainModule.GetType("System.DateTime");
        foreach (var m in dt.Methods) {
            if (!m.HasBody) continue;
            bool hasOOR = false;
            foreach (var inst in m.Body.Instructions) {
                if (inst.OpCode == OpCodes.Newobj && inst.Operand is MethodReference mr && mr.DeclaringType.Name == "ArgumentOutOfRangeException") {
                    hasOOR = true;
                    break;
                }
            }
            if (hasOOR) {
                Console.WriteLine("=== " + m.FullName + " ===");
                foreach (var inst in m.Body.Instructions) {
                    if (inst.OpCode == OpCodes.Ldstr) {
                        Console.WriteLine($"   ldstr \"{inst.Operand}\"");
                    }
                }
            }
        }
    }
}
