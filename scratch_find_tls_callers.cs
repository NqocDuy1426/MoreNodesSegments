
using System;
using Mono.Cecil;
class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        foreach (var t in asm.MainModule.Types) {
            foreach (var m in t.Methods) {
                if (m.HasBody) {
                    foreach (var inst in m.Body.Instructions) {
                        if (inst.Operand is MethodReference mr && (mr.Name.Contains("TrafficLight") || mr.Name.Contains("Bollard"))) {
                            Console.WriteLine(t.FullName + "." + m.Name + " -> " + mr.DeclaringType.Name + "." + mr.Name);
                        }
                    }
                }
            }
        }
    }
}
