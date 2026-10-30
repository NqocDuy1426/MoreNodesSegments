
using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Dump
{
    static void Main()
    {
        string gameDir = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(Path.Combine(gameDir, "Assembly-CSharp.dll"));
        foreach (var t in asm.MainModule.Types)
        {
            foreach (var m in t.Methods)
            {
                if (m.HasBody)
                {
                    foreach (var inst in m.Body.Instructions)
                    {
                        if (inst.Operand is FieldReference fr && (fr.Name == "m_firstVehicle" || fr.Name == "m_nextLaneVehicle"))
                        {
                            Console.WriteLine(string.Format("{0}.{1} accesses {2} (OpCode: {3})", t.FullName, m.Name, fr.Name, inst.OpCode));
                        }
                    }
                }
            }
        }
    }
}
