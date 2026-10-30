using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var gameDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");

        foreach (var t in asm.MainModule.Types)
        {
            foreach (var m in t.Methods)
            {
                if (m.Body == null) continue;
                foreach (var inst in m.Body.Instructions)
                {
                    var fr = inst.Operand as FieldReference;
                    if (fr != null && fr.DeclaringType.Name == "NetSegment")
                    {
                        if (fr.Name == "m_trafficDensity" || fr.Name == "m_trafficBuffer")
                        {
                            Console.WriteLine($"{t.Name}::{m.Name} references {fr.Name}");
                            break;
                        }
                    }
                }
            }
        }
    }
}
