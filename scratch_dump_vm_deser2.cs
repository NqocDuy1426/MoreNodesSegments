using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var vm = asm.MainModule.GetType("VehicleManager");
        foreach (var nested in vm.NestedTypes) {
            if (nested.Name == "Data") {
                foreach (var m in nested.Methods) {
                    if (m.Name == "Deserialize") {
                        int count = 0;
                        foreach (var instr in m.Body.Instructions) {
                            if (instr.OpCode.ToString().StartsWith("ldfld") || 
                                instr.OpCode.ToString().StartsWith("stfld") ||
                                instr.OpCode.ToString().StartsWith("call") ||
                                instr.OpCode == OpCodes.Ldstr) {
                                Console.WriteLine(string.Format("  IL_{0:X4}: {1} {2}", instr.Offset, instr.OpCode, instr.Operand));
                                if (++count > 100) break;
                            }
                        }
                    }
                }
            }
        }
    }
}
