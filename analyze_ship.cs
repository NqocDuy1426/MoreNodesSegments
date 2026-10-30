using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void PrintMethod(TypeDefinition type, string methodName) {
        Console.WriteLine($"\n================== {type.Name}.{methodName} ==================");
        foreach (var m in type.Methods) {
            if (m.Name == methodName) {
                Console.WriteLine($"Method: {m.FullName}");
                if (m.Body != null) {
                    foreach (var ins in m.Body.Instructions) {
                        string opStr = ins.Operand != null ? ins.Operand.ToString() : "";
                        if (ins.Operand is Instruction target) opStr = $"IL_{target.Offset:x4}";
                        Console.WriteLine($"IL_{ins.Offset:x4}: {ins.OpCode} {opStr}");
                    }
                }
            }
        }
    }

    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var tt = asm.MainModule.GetType("TransportTool");
        PrintMethod(tt, "GetStopPosition");
        PrintMethod(tt, "CanAddStop");
        PrintMethod(tt, "AddStop");

        var sd = asm.MainModule.GetType("ShipDockAI");
        PrintMethod(sd, "RemoveLaneConnection");

        var sp = asm.MainModule.GetType("ShipPathAI");
        PrintMethod(sp, "UpdateLanes");
    }
}
