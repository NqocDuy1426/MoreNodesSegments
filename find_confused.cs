using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class FindConfused
{
    static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(Path.Combine(gameDir, "Assembly-CSharp.dll"));
        foreach (var type in asm.MainModule.Types)
        {
            foreach (var m in type.Methods)
            {
                if (m.HasBody)
                {
                    foreach (var inst in m.Body.Instructions)
                    {
                        if (inst.Operand is string s && s.Contains("CONFUSED"))
                        {
                            Console.WriteLine(string.Format("{0}.{1} uses {2}", type.FullName, m.Name, s));
                        }
                    }
                }
            }
        }
    }
}
