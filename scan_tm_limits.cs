using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

public class ScanTmHardcodedLimits
{
    public static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        CheckType(asm.MainModule.GetType("TransportManager"));
        CheckType(asm.MainModule.GetType("TransportLine"));
        CheckType(asm.MainModule.GetType("BusAI"));
        CheckType(asm.MainModule.GetType("PassengerCarAI"));
    }

    static void CheckType(TypeDefinition t)
    {
        if (t == null) return;
        Console.WriteLine("\nChecking type: " + t.FullName);
        foreach (var m in t.Methods)
        {
            if (!m.HasBody) continue;
            foreach (var instr in m.Body.Instructions)
            {
                if (instr.OpCode == OpCodes.Ldc_I4)
                {
                    int val = (int)instr.Operand;
                    if (val == 32768 || val == 36864 || val == 49152 || val == 65536)
                    {
                        Console.WriteLine(string.Format("  {0} -> {1} {2}", m.Name, instr.OpCode, val));
                    }
                }
            }
        }
    }
}
