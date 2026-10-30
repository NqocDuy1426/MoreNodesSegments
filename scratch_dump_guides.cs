using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var nm = asm.MainModule.Types.First(x => x.Name == "NetManager");
        var data = nm.NestedTypes.First(x => x.Name == "Data");
        var des = data.Methods.First(x => x.Name == "Deserialize");

        foreach (var ins in des.Body.Instructions) {
            string s = ins.ToString();
            if (s.Contains("m_guideInfoIndex") || s.Contains("m_milestoneInfoIndex")) {
                Console.WriteLine($"{ins.Offset:X4}: {ins.OpCode} {ins.Operand}");
            }
        }
    }
}
