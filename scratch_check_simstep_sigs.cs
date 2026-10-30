using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        string[] types = new string[] { "VehicleAI", "CarAI", "PassengerCarAI" };
        foreach (var name in types) {
            var t = asm.MainModule.GetType(name);
            if (t != null) {
                foreach (var m in t.Methods) {
                    if (m.Name.Contains("SimulationStep")) {
                        Console.WriteLine(name + "." + m.Name + " (" + m.Parameters.Count + " params)");
                        foreach (var p in m.Parameters) Console.WriteLine("    " + p.ParameterType.FullName + " " + p.Name);
                    }
                }
            }
        }
    }
}
