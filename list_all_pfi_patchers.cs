using System;
using System.IO;
using Mono.Cecil;

class Program {
    static void Main() {
        string[] searchDirs = new string[] {
            "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710",
            "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods"
        };
        foreach (var dir in searchDirs) {
            if (!Directory.Exists(dir)) continue;
            foreach (var dll in Directory.GetFiles(dir, "*.dll", SearchOption.AllDirectories)) {
                try {
                    var asm = AssemblyDefinition.ReadAssembly(dll);
                    foreach (var type in asm.MainModule.Types) {
                        CheckType(dll, type);
                    }
                } catch {}
            }
        }
    }

    static void CheckType(string dll, TypeDefinition type) {
        foreach (var ca in type.CustomAttributes) {
            foreach (var arg in ca.ConstructorArguments) {
                if (arg.Value != null && arg.Value.ToString().Contains("PathFindImplementation")) {
                    Console.WriteLine($"[Harmony attribute on type] {Path.GetFileName(dll)} -> {type.FullName}: {arg.Value}");
                }
            }
        }
        foreach (var m in type.Methods) {
            foreach (var ca in m.CustomAttributes) {
                foreach (var arg in ca.ConstructorArguments) {
                    if (arg.Value != null && arg.Value.ToString().Contains("PathFindImplementation")) {
                        Console.WriteLine($"[Harmony attribute on method] {Path.GetFileName(dll)} -> {type.FullName}::{m.Name}: {arg.Value}");
                    }
                }
            }
            if (m.HasBody) {
                foreach (var ins in m.Body.Instructions) {
                    if (ins.Operand is string s && s.Contains("PathFindImplementation")) {
                        Console.WriteLine($"[String PathFindImplementation] in {Path.GetFileName(dll)} -> {type.FullName}.{m.Name}: {ins}");
                    }
                }
            }
        }
        foreach (var nested in type.NestedTypes) CheckType(dll, nested);
    }
}
