using System;
using Mono.Cecil;
class P {
  static void Main() {
    string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
    var asm = AssemblyDefinition.ReadAssembly(path);
    var nm = asm.MainModule.GetType("NetManager");
    foreach (var m in nm.Methods) {
      if (m.Name == "UpdateSegmentRenderer") {
        Console.WriteLine(m.FullName);
        foreach (var p in m.Parameters) Console.WriteLine($"  Param: {p.ParameterType} {p.Name}");
      }
    }
  }
}
