using System;
using System.IO;
using System.Linq;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using Mono.Cecil;

class Program {
    static void Main() {
        var resolver = new UniversalAssemblyResolver(
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll",
            false,
            ".NETFramework,Version=v3.5");
        resolver.AddSearchDirectory("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed");
        
        var decompiler = new CSharpDecompiler(
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll",
            resolver,
            new DecompilerSettings());
        
        var name = new ICSharpCode.Decompiler.TypeSystem.FullTypeName("TrafficManager.Manager.Impl.TrafficPriorityManager");
        var typeInfo = decompiler.TypeSystem.FindType(name);
        foreach (var method in typeInfo.GetMethods(m => m.Name == "HasPriority")) {
            Console.WriteLine(decompiler.DecompileAsString(method.MetadataToken));
        }
    }
}
