
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var st = asm.MainModule.GetType("Starter");
        var m = st.Methods;
        foreach (var method in m) {
            if (method.Name == "Awake") {
                foreach (var inst in method.Body.Instructions) {
                    if (inst.Operand != null && inst.Operand.ToString().Contains("RegisterManager")) {
                        Console.WriteLine(inst);
                    } else if (inst.OpCode.ToString().Contains("call") || inst.OpCode.ToString().Contains("ldsfld")) {
                        Console.WriteLine("  " + inst);
                    }
                }
            }
        }
    }
}
