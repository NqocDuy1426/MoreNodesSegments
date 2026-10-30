using System;
using Mono.Cecil;
class P {
  static void Main() {
    var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TMPE.API.dll");
    var evt = asm.MainModule.GetType("TrafficManager.API.Traffic.Enums.ExtVehicleType");
    Console.WriteLine("Type: " + evt.FullName);
    foreach (var f in evt.Fields) {
      if (f.IsStatic) Console.WriteLine($"  {f.Name} = {f.Constant}");
    }
  }
}
