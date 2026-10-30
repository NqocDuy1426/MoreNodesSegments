using System;
using Mono.Cecil;
class P {
  static void Main() {
    var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
    var slm = asm.MainModule.GetType("TrafficManager.Manager.Impl.SpeedLimitManager");
    Console.WriteLine("Type: " + slm.FullName);
    foreach (var f in slm.Fields) Console.WriteLine("  Field: " + f.Name + " (" + f.FieldType + ")");
    foreach (var m in slm.Methods) {
      if (m.Name.Contains("LoadData") || m.Name.Contains("Awake") || m.Name.Contains("Init") || m.Name.Contains(".ctor")) {
        Console.WriteLine("\nMethod: " + m.FullName);
      }
    }
  }
}
