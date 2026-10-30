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

        foreach (var type in asm.MainModule.Types)
        {
            SearchType(type);
        }
    }

    static void SearchType(TypeDefinition type)
    {
        foreach (var nested in type.NestedTypes)
            SearchType(nested);

        foreach (var method in type.Methods)
        {
            if (method.Body == null) continue;
            foreach (var inst in method.Body.Instructions)
            {
                var mr = inst.Operand as MethodReference;
                if (mr != null && mr.DeclaringType.Name == "EconomyManager")
                {
                    if (mr.Name.Contains("AddResource") || mr.Name.Contains("FetchResource") || mr.Name.Contains("AddPrivateIncome"))
                    {
                        if (type.Name.Contains("Train") || type.Name.Contains("TransportLine") || type.Name.Contains("Passenger") || type.Name.Contains("Cargo"))
                        {
                            Console.WriteLine($"{type.Name}::{method.Name} -> {mr.Name}");
                        }
                    }
                }
            }
        }
    }
}
