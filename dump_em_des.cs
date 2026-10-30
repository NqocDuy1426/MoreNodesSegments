
using System;
using System.Reflection;
using Mono.Cecil;
class P {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var t = asm.MainModule.GetType("ElectricityManager");
        Console.WriteLine("ElectricityManager fields:");
        foreach (var f in t.Fields) {
            if (f.FieldType.IsArray) Console.WriteLine($"  {f.Name}: {f.FieldType.FullName}");
        }
        var dt = asm.MainModule.GetType("ElectricityManager/Data");
        var m = System.Linq.Enumerable.First(dt.Methods, x => x.Name == "Deserialize");
        Console.WriteLine($"Method: {m.FullName}");
        foreach (var i in m.Body.Instructions) {
            if (i.Operand != null && (i.Operand.ToString().Contains("m_") || i.Operand.ToString().Contains("Read") || i.Operand.ToString().Contains("Deserialize"))) {
                Console.WriteLine($"  {i.Offset:X4}: {i.OpCode} {i.Operand}");
            }
        }
    }
}
