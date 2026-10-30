using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("MoreNetworkCapacity.Core.dll");
        string[] targetTypes = new string[] {
            "NetManagerColorUpdatePatch",
            "NetManagerUpdateNodeColorsPatch",
            "NetManagerUpdateSegmentColorsPatch",
            "NetworkColorOffsetPatch",
            "RenderGroupMeshDataPatch",
            "RenderManagerAwakePatch"
        };

        foreach (var tName in targetTypes)
        {
            var t = asm.MainModule.GetType("MoreNetworkCapacity.Core." + tName) ?? asm.MainModule.GetType(tName);
            if (t == null)
            {
                foreach (var t2 in asm.MainModule.Types)
                {
                    if (t2.Name == tName) { t = t2; break; }
                }
            }
            if (t != null)
            {
                Console.WriteLine($"\n==================== {t.FullName} ====================");
                foreach (var m in t.Methods)
                {
                    Console.WriteLine($"--- Method: {m.Name} ---");
                    if (m.Body != null)
                    {
                        foreach (var inst in m.Body.Instructions)
                        {
                            Console.WriteLine($"  IL_{inst.Offset:X4}: {inst.OpCode} {inst.Operand}");
                        }
                    }
                }
            }
        }
    }
}
