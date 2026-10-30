using System;
using Mono.Cecil;
class P {
  static void Main() {
    var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
    var vrm = asm.MainModule.GetType("TrafficManager.Manager.Impl.VehicleRestrictionsManager");
    foreach (var m in vrm.Methods) {
      if (m.Name == "GetDefaultAllowedVehicleTypes" && m.Parameters.Count == 5) {
        Console.WriteLine(m.FullName);
        for (int i = 0; i < 40; i++) Console.WriteLine("  " + m.Body.Instructions[i]);
      }
    }
  }
}
