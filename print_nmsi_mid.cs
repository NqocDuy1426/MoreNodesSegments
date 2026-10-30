using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var nm = asm.MainModule.GetType("NetManager");
        foreach (var m in nm.Methods) {
            if (m.Name == "SimulationStepImpl") {
                bool print = false;
                foreach (var inst in m.Body.Instructions) {
                    if (inst.Offset >= 0x0460) print = true;
                    if (inst.Offset >= 0x06c0) break;
                    if (print) Console.WriteLine(inst);
                }
            }
        }
    }
}
