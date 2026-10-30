
using System;
using Mono.Cecil;
class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/ColossalManaged.dll");
        var t = asm.MainModule.GetType("ColossalFramework.UI.UILabel");
        foreach (var m in t.Methods) {
            if (m.Name == "set_text") {
                Console.WriteLine("UILabel.set_text");
                foreach (var inst in m.Body.Instructions) {
                    Console.WriteLine("  {0:X4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand);
                }
            }
        }
        var t2 = asm.MainModule.GetType("ColossalFramework.UI.UITextField");
        foreach (var m in t2.Methods) {
            if (m.Name == "set_text") {
                Console.WriteLine("UITextField.set_text");
                foreach (var inst in m.Body.Instructions) {
                    Console.WriteLine("  {0:X4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand);
                }
            }
        }
    }
}
