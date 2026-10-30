using System;
using System.IO;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        foreach (var dll in Directory.GetFiles(".", "*.dll"))
        {
            try
            {
                var asm = AssemblyDefinition.ReadAssembly(dll);
                foreach (var module in asm.Modules)
                {
                    foreach (var type in module.Types)
                    {
                        CheckType(type, dll);
                    }
                }
            }
            catch {}
        }
    }

    static void CheckType(TypeDefinition t, string dll)
    {
        foreach (var ca in t.CustomAttributes)
        {
            if (ca.AttributeType.Name == "HarmonyPatch")
            {
                foreach (var arg in ca.ConstructorArguments)
                {
                    if (arg.Value != null && (arg.Value.ToString().Contains("Deserialize") || arg.Value.ToString().Contains("NetManager")))
                    {
                        Console.WriteLine($"[HarmonyPatch] {dll} -> {t.FullName} attr arg: {arg.Value}");
                    }
                }
            }
        }
        foreach (var m in t.Methods)
        {
            if (m.HasBody)
            {
                foreach (var inst in m.Body.Instructions)
                {
                    if (inst.Operand != null && inst.Operand.ToString().Contains("Deserialize") && inst.Operand.ToString().Contains("NetManager"))
                    {
                        Console.WriteLine($"[Instruction] {dll} -> {t.FullName}.{m.Name}: {inst.OpCode} {inst.Operand}");
                    }
                }
            }
        }
        foreach (var nested in t.NestedTypes)
        {
            CheckType(nested, dll);
        }
    }
}
