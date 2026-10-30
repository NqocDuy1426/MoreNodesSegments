
using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/content_beta/MoreNodesSegmentsBeta.dll");
        var t = asm.MainModule.GetType("MoreNetworkCapacityRenew.TransportLineCompatibility");
        var m = t.Methods;
        foreach (var method in m) {
            if (method.Name == "SimulationManager_Managers_UpdateData_Prefix") {
                Console.WriteLine("Method: " + method.FullName);
                foreach (var inst in method.Body.Instructions) {
                    Console.WriteLine(string.Format("IL_{0:X4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand));
                }
            }
        }
    }
}
