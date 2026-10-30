
using System;
using Mono.Cecil;
class P {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/ColossalManaged.dll");
        var t = asm.MainModule.GetType("ColossalFramework.IO.EncodedArray/UShort");
        foreach (var m in t.Methods) {
            if (m.Name == "Read" && m.Parameters.Count > 1) {
                Console.WriteLine(m.FullName);
                foreach (var i in m.Body.Instructions) Console.WriteLine("  " + i);
            }
        }
    }
}
