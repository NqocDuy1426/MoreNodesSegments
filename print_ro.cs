using System;
using System.Reflection;
using System.Collections.Generic;

class Program {
    static void Main() {
        var asm = Assembly.LoadFrom("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/ColossalManaged.dll");
        var t = asm.GetType("ColossalFramework.IO.DataSerializer");
        foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
            if (m.Name == "ReadObjectArray" || m.Name == "ReadObject" || m.Name == "ReadSharedType") {
                Console.WriteLine("Method: " + m.Name + " (" + string.Join(", ", Array.ConvertAll(m.GetParameters(), p => p.ParameterType.Name)) + ")");
            }
        }
    }
}
