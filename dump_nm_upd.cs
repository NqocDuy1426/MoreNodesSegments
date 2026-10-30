
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var nm = asm.MainModule.GetType("NetManager");
        foreach (var m in nm.Methods) {
            if (m.Name == "UpdateData") {
                Console.WriteLine("Method: " + m.FullName);
                for (int i = 0; i < Math.Min(m.Body.Instructions.Count, 30); i++) {
                    Console.WriteLine("  " + m.Body.Instructions[i]);
                }
            }
        }
    }
}
