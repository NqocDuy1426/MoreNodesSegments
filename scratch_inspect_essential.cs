using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var lm = asm.MainModule.GetType("LoadingManager");
        foreach (var m in lm.Methods.Where(x => x.Name == "EssentialScenesLoaded" || x.Name == "RenderDataReady" || x.Name.Contains("SimulationDataLoaded"))) {
            Console.WriteLine("Method: " + m.Name);
        }
        foreach (var t in lm.NestedTypes.Where(x => x.Name.Contains("EssentialScenesLoaded") || x.Name.Contains("RenderDataReady"))) {
            Console.WriteLine("Nested: " + t.FullName);
            var mn = t.Methods.FirstOrDefault(x => x.Name == "MoveNext");
            if (mn != null) {
                foreach (var inst in mn.Body.Instructions) {
                    Console.WriteLine("  " + inst);
                }
            }
        }
    }
}

