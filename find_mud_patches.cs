using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        string[] dirs = new string[] {
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods",
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/Steam/steamapps/workshop/content/255710"
        };
        foreach (var dir in dirs) {
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
    static void CheckType(string dll, TypeDefinition t) {
        foreach (var m in t.Methods) {
            if (m.HasBody) {
                foreach (var ins in m.Body.Instructions) {
                    if (ins.Operand != null && ins.Operand.ToString().Contains("Managers_UpdateData")) {
                        Console.WriteLine($"[FOUND] {Path.GetFileName(dll)} -> {t.FullName}::{m.Name} references {ins.Operand}");
                    }
                }
            }
        }
        foreach (var nested in t.NestedTypes) {
            CheckType(dll, nested);
        }
    }
}
