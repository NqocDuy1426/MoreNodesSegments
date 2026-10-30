
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var wm = asm.MainModule.GetType("WeatherManager");
        Console.WriteLine("BaseType: " + wm.BaseType.FullName);
        var baseType = wm.BaseType.Resolve();
        while (baseType != null) {
            Console.WriteLine("Base: " + baseType.FullName);
            foreach (var m in baseType.Methods) {
                if (m.Name.Contains("UpdateData")) {
                    Console.WriteLine("  " + m.FullName);
                    foreach (var inst in m.Body.Instructions) {
                        Console.WriteLine("    " + inst);
                    }
                }
            }
            if (baseType.BaseType == null) break;
            baseType = baseType.BaseType.Resolve();
        }
    }
}
