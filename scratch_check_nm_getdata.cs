using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var nm = asm.MainModule.Types.First(t => t.Name == "NetManager");
        var m = nm.Methods.FirstOrDefault(x => x.Name.Contains("GetData"));
        if (m != null) {
            Console.WriteLine("NetManager.GetData:");
            foreach (var ins in m.Body.Instructions) {
                Console.WriteLine($"  {ins.Offset:X4}: {ins.OpCode} {ins.Operand}");
            }
        }
    }
}
