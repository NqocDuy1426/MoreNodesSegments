using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        string[] mods = new string[] {
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/924884948/PlopTheGrowables.dll",
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/3702700108/NetworkSkins.dll",
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/552309905/NoAbandonment.dll"
        };
        foreach (var mod in mods) {
            if (!File.Exists(mod)) continue;
            var asm = AssemblyDefinition.ReadAssembly(mod);
            Console.WriteLine("=== Mod: " + Path.GetFileName(mod) + " ===");
            foreach (var t in asm.MainModule.Types) {
                foreach (var m in t.Methods) {
                    if (m.HasBody) {
                        foreach (var ins in m.Body.Instructions) {
                            if (ins.Operand != null && ins.Operand.ToString().Contains("Managers_UpdateData")) {
                                Console.WriteLine($"  {t.FullName}::{m.Name} -> {ins}");
                            }
                        }
                    }
                }
                foreach (var nt in t.NestedTypes) {
                    foreach (var m in nt.Methods) {
                        if (m.HasBody) {
                            foreach (var ins in m.Body.Instructions) {
                                if (ins.Operand != null && ins.Operand.ToString().Contains("Managers_UpdateData")) {
                                    Console.WriteLine($"  {nt.FullName}::{m.Name} -> {ins}");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
