
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var type = asm.MainModule.GetType("NetSegment");
        foreach (var nested in type.NestedTypes) {
            if (nested.Name == "Flags") {
                foreach (var f in nested.Fields) {
                    if (f.HasConstant) Console.WriteLine($"{f.Name} = {f.Constant} (0x{(int)f.Constant:X})");
                }
            }
        }
    }
}
