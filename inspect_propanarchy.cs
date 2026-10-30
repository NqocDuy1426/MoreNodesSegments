using System;
using System.Linq;
using Mono.Cecil;

class Program {
    static void Main() {
        string[] paths = new string[] {
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/593588108/PropAnarchy.dll",
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/2862882210/PropAnarchy.dll"
        };
        foreach (var p in paths) {
            try {
                var asm = AssemblyDefinition.ReadAssembly(p);
                Console.WriteLine($"{p}: {asm.Name.Version}");
            } catch (Exception ex) {
                Console.WriteLine($"{p}: error {ex.Message}");
            }
        }
    }
}
