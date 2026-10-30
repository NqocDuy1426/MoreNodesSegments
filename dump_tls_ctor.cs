
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        var t = asm.MainModule.GetType("TrafficManager.TrafficLight.Impl.TrafficLightSimulation");
        foreach (var ctor in t.Methods) {
            if (ctor.IsConstructor) {
                Console.WriteLine("Ctor: " + ctor.FullName);
                foreach (var ins in ctor.Body.Instructions) {
                    Console.WriteLine("  " + ins);
                }
            }
        }
    }
}
