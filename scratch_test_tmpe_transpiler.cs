using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(path);
        int changed = 0;
        foreach (var t in asm.MainModule.Types)
        {
            if (t.FullName.StartsWith("TrafficManager.Custom.PathFinding.")) continue;
            foreach (var m in t.Methods)
            {
                if (!m.HasBody) continue;
                var insts = m.Body.Instructions;
                for (int i = 0; i < insts.Count; i++)
                {
                    if (insts[i].OpCode == OpCodes.Ldc_I4)
                    {
                        int val = (int)insts[i].Operand;
                        if (val == 36864 || val == 32768 || val == 36863 || val == 32767 || val == 73728 || val == 73727)
                        {
                            bool shouldChange = false;
                            for (int j = i + 1; j < Math.Min(i + 3, insts.Count); j++)
                            {
                                var nextOp = insts[j].OpCode;
                                if (IsBoundsCheckOp(nextOp) || IsAllocationOp(nextOp) || nextOp == OpCodes.Stfld || nextOp == OpCodes.Stsfld)
                                {
                                    shouldChange = true;
                                    break;
                                }
                            }
                            if (shouldChange)
                            {
                                Console.WriteLine($"WILL EXPAND: {t.FullName}.{m.Name} val {val}");
                                changed++;
                            }
                        }
                    }
                }
            }
        }
        Console.WriteLine($"Total instructions that will be expanded in TM:PE: {changed}");
    }

    static bool IsBoundsCheckOp(OpCode op)
    {
        return op == OpCodes.Blt || op == OpCodes.Blt_S || op == OpCodes.Blt_Un || op == OpCodes.Blt_Un_S
            || op == OpCodes.Ble || op == OpCodes.Ble_S || op == OpCodes.Ble_Un || op == OpCodes.Ble_Un_S
            || op == OpCodes.Bgt || op == OpCodes.Bgt_S || op == OpCodes.Bgt_Un || op == OpCodes.Bgt_Un_S
            || op == OpCodes.Bge || op == OpCodes.Bge_S || op == OpCodes.Bge_Un || op == OpCodes.Bge_Un_S
            || op == OpCodes.Bne_Un || op == OpCodes.Bne_Un_S
            || op == OpCodes.Beq || op == OpCodes.Beq_S
            || op == OpCodes.Clt || op == OpCodes.Clt_Un
            || op == OpCodes.Cgt || op == OpCodes.Cgt_Un
            || op == OpCodes.Ceq;
    }

    static bool IsAllocationOp(OpCode op)
    {
        return op == OpCodes.Newarr || op == OpCodes.Newobj;
    }
}
