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
                        Console.WriteLine("VehicleManager.Data.Deserialize (" + m.Parameters.Count + " params)");
                        foreach (var instr in m.Body.Instructions) {
                            Console.WriteLine(string.Format("  IL_{0:X4}: {1} {2}", instr.Offset, instr.OpCode, instr.Operand));
                        }
                    }
                }
            }
        }
    }
}
