using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var gameDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");
        
        Console.WriteLine("Searching for PublicTransportTrain usages in Assembly-CSharp...");
        foreach (var type in asm.MainModule.Types)
        {
            foreach (var m in type.Methods)
            {
                if (m.Body == null) continue;
                for (int i = 0; i < m.Body.Instructions.Count; i++)
                {
                    var inst = m.Body.Instructions[i];
                    if ((inst.OpCode == OpCodes.Ldc_I4_S && (sbyte)inst.Operand == 11) ||
                        (inst.OpCode == OpCodes.Ldc_I4 && (int)inst.Operand == 11))
                    {
                        string typeName = type.FullName;
                        string mName = m.Name;
                        if (typeName.Contains("Economy") || typeName.Contains("Transport") || typeName.Contains("Train") || typeName.Contains("NetAI") || typeName.Contains("Building"))
                        {
                            Console.WriteLine($"{typeName}.{mName} at IL_{inst.Offset:X4}");
                        }
                    }
                }
            }
        }
    }
}
