
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/content_beta/MoreNodesSegmentsBeta.dll");
        var t = asm.MainModule.GetType("MoreNetworkCapacityRenew.TransportLineCompatibility");
        foreach (var m in t.Methods) {
            if (m.Name.Contains("Managers_UpdateData")) {
                Console.WriteLine("Method: " + m.FullName);
                foreach (var inst in m.Body.Instructions) {
                    Console.WriteLine("  " + inst);
                }
            }
        }
    }
}
