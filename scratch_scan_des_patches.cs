using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string dir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710";
        var dlls = Directory.GetFiles(dir, "*.dll", SearchOption.AllDirectories);
        Console.WriteLine("Scanning " + dlls.Length + " DLLs...");
        foreach (var dll in dlls)
        {
            try
            {
                var asm = AssemblyDefinition.ReadAssembly(dll);
                foreach (var mod in asm.Modules)
                {
                    foreach (var type in mod.Types)
                    {
                        foreach (var m in type.Methods)
                        {
                            if (m.HasBody)
                            {
                                foreach (var inst in m.Body.Instructions)
                                {
                                    if (inst.OpCode == OpCodes.Ldstr)
                                    {
                                        string s = inst.Operand as string;
                                        if (s == "Deserialize" || s == "NetManager")
                                        {
                                            // check if both appear in method
                                            bool hasBoth = false;
                                            foreach (var inst2 in m.Body.Instructions)
                                            {
                                                if (inst2.OpCode == OpCodes.Ldstr && (inst2.Operand as string) == (s == "Deserialize" ? "NetManager" : "Deserialize"))
                                                {
                                                    hasBoth = true;
                                                    break;
                                                }
                                            }
                                            if (hasBoth)
                                            {
                                                Console.WriteLine("Match in " + Path.GetFileName(dll) + " -> " + type.FullName + "." + m.Name);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch {}
        }
    }
}
