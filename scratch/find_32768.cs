using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var assembly = AssemblyDefinition.ReadAssembly(path);
        
        foreach (var t in assembly.MainModule.Types)
        {
            ScanType(t);
        }
    }

    static void ScanType(TypeDefinition t)
    {
        foreach (var m in t.Methods)
        {
            if (m.Body != null)
            {
                for (int i = 0; i < m.Body.Instructions.Count; i++)
                {
                    var ins = m.Body.Instructions[i];
                    if (ins.OpCode == OpCodes.Ldc_I4 && Convert.ToInt32(ins.Operand) == 32768)
                    {
                        // Check if previous or next instructions involve division or shift
                        Console.WriteLine(t.FullName + "::" + m.Name + " has ldc.i4 32768 at offset " + ins.Offset.ToString("X4"));
                        int start = Math.Max(0, i - 3);
                        int end = Math.Min(m.Body.Instructions.Count - 1, i + 3);
                        for (int k = start; k <= end; k++)
                        {
                            Console.WriteLine("    " + m.Body.Instructions[k]);
                        }
                    }
                }
            }
        }
        foreach (var nested in t.NestedTypes)
        {
            ScanType(nested);
        }
    }
}
