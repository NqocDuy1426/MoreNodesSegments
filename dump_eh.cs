
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
                Console.WriteLine("Exception Handlers:");
                foreach (var eh in method.Body.ExceptionHandlers) {
                    Console.WriteLine(string.Format("Type: {0}, Try: {1:X4}..{2:X4}, Handler: {3:X4}..{4:X4}", 
                        eh.HandlerType, eh.TryStart.Offset, eh.TryEnd.Offset, eh.HandlerStart.Offset, eh.HandlerEnd != null ? eh.HandlerEnd.Offset : 0));
                }
            }
        }
    }
}
