using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        string mod = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/3702700108/NetworkSkins.dll";
        var asm = AssemblyDefinition.ReadAssembly(mod);
        foreach (var t in asm.MainModule.Types) {
            foreach (var ca in t.CustomAttributes) {
                if (ca.AttributeType.Name.Contains("Harmony")) {
                    Console.WriteLine($"[Attr] {t.FullName}: {ca.AttributeType.Name} args: " + string.Join(", ", ca.ConstructorArguments.Select(a => a.Value?.ToString() ?? "null")));
                }
            }
            foreach (var m in t.Methods) {
                foreach (var ca in m.CustomAttributes) {
                    if (ca.AttributeType.Name.Contains("Harmony")) {
                        Console.WriteLine($"[Attr] {t.FullName}::{m.Name}: {ca.AttributeType.Name} args: " + string.Join(", ", ca.ConstructorArguments.Select(a => a.Value?.ToString() ?? "null")));
                    }
                }
            }
        }
    }
}
