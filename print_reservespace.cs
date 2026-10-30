using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var nl = asm.MainModule.GetType("NetLane");
        foreach (var m in nl.Methods) {
            if (m.Name.Contains("ReserveSpace")) {
                Console.WriteLine("Method: " + m.FullName);
                foreach (var inst in m.Body.Instructions) {
                    Console.WriteLine("  " + inst);
                }
            }
        }
    }
}
