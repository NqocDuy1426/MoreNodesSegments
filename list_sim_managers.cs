using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var resolver = new DefaultAssemblyResolver();
        string home = Environment.GetEnvironmentVariable("HOME");
        string gameDir = home + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        resolver.AddSearchDirectory(gameDir);
        var p = new ReaderParameters { AssemblyResolver = resolver };
        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll", p);
        
        foreach (var t in asm.MainModule.Types) {
            if (t.Interfaces.Any(i => i.InterfaceType.Name == "ISimulationManager")) {
                Console.WriteLine("ISimulationManager: " + t.FullName);
                var getData = t.Methods.FirstOrDefault(m => m.Name == "GetData");
                if (getData != null) {
                    foreach (var inst in getData.Body.Instructions) {
                        if (inst.OpCode == OpCodes.Newobj) {
                            Console.WriteLine("    Creates Data: " + inst.Operand);
                        }
                    }
                }
            }
        }
    }
}
