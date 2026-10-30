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
                    if (mr.Name.Contains("AddResource") || mr.Name.Contains("FetchResource"))
                    {
                        // Check if instructions load PublicTransport
                        bool pt = false;
                        foreach (var inst2 in method.Body.Instructions)
                        {
                            if (inst2.OpCode == OpCodes.Ldc_I4 || inst2.OpCode == OpCodes.Ldc_I4_S)
                            {
                                int val = Convert.ToInt32(inst2.Operand);
                                // PublicTransport service = 9, subservice train = 11, Resource.PublicTransport = 13 or similar
                                if (val == 9 || val == 11) pt = true;
                            }
                        }
                        if (pt)
                        {
                            Console.WriteLine($"{type.Name}::{method.Name} calls {mr.Name}");
                            break;
                        }
                    }
                }
            }
        }
    }
}
