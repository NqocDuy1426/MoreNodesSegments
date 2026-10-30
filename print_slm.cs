using System;
using Mono.Cecil;
class P {
  static void Main() {
    var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
    var slm = asm.MainModule.GetType("TrafficManager.Manager.Impl.SpeedLimitManager");
    foreach (var m in slm.Methods) {
      if (m.Name == "GetGameSpeedLimit") {
        Console.WriteLine("\nMethod: " + m.FullName);
        foreach (var ins in m.Body.Instructions) {
          Console.WriteLine("  " + ins);
        }
      }
    }
  }
}
