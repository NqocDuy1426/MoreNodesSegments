using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void PrintMethod(MethodDefinition m) {
        Console.WriteLine($"\n================== {m.DeclaringType.Name}.{m.Name} ==================");
        foreach (var ins in m.Body.Instructions) {
            string opStr = ins.Operand != null ? ins.Operand.ToString() : "";
            if (ins.Operand is Instruction target) opStr = $"IL_{target.Offset:x4}";
            Console.WriteLine($"IL_{ins.Offset:x4}: {ins.OpCode} {opStr}");
        }
    }

    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var tt = asm.MainModule.GetType("TransportTool");
        foreach (var nested in tt.NestedTypes) {
            if (nested.Name.Contains("NewLine") || nested.Name.Contains("AddStop")) {
                foreach (var m in nested.Methods) {
                    if (m.Name == "MoveNext") PrintMethod(m);
                }
            }
        }
    }
}
