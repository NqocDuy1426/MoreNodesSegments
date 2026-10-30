
using System;
using Mono.Cecil;
class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        foreach (var t in asm.MainModule.Types) {
            if (t.BaseType != null && t.BaseType.Name.Contains("WorldInfoPanel")) {
                Console.WriteLine(t.FullName);
            }
        }
    }
}
