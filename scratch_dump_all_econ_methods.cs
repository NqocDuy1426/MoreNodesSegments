using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var gameDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");

        var type = asm.MainModule.GetType("EconomyManager");
        foreach (var m in type.Methods)
        {
            if (m.Name == "FetchResource" || m.Name == "AddResource" || m.Name == "PeekResource" || m.Name == "AddPrivateIncome")
            {
                Console.Write($"{m.ReturnType.Name} {m.Name}(");
                for (int i = 0; i < m.Parameters.Count; i++)
                {
                    if (i > 0) Console.Write(", ");
                    Console.Write($"{m.Parameters[i].ParameterType.Name} {m.Parameters[i].Name}");
                }
                Console.WriteLine(")");
            }
        }
    }
}
