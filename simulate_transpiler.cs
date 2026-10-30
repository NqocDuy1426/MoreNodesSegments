using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
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

    static bool IsPreserveOp(OpCode op)
    {
        return op == OpCodes.And || op == OpCodes.Or || op == OpCodes.Xor
            || op == OpCodes.Div || op == OpCodes.Div_Un
            || op == OpCodes.Rem || op == OpCodes.Rem_Un
            || op == OpCodes.Mul
            || op == OpCodes.Stloc || op == OpCodes.Stloc_S
            || op == OpCodes.Stloc_0 || op == OpCodes.Stloc_1 || op == OpCodes.Stloc_2 || op == OpCodes.Stloc_3
            || op == OpCodes.Call || op == OpCodes.Callvirt;
    }

    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var asm = AssemblyDefinition.ReadAssembly(tmpePath);
        var type = asm.MainModule.GetType("TrafficManager.Custom.PathFinding.CustomPathFind");
        var m = type.Methods;
        foreach (var method in m)
        {
            if (method.Name == "PathFindImplementation")
            {
                var codes = method.Body.Instructions;
                Console.WriteLine("Simulating ValidationTranspiler on PathFindImplementation...");
                for (int i = 0; i < codes.Count; i++)
                {
                    var instruction = codes[i];
                    if (instruction.OpCode == OpCodes.Ldc_I4)
                    {
                        int val = (int)instruction.Operand;
                        bool isPathFindOrRouting = true;
                        if (val == 36864 || val == 32768 || val == 36863 || val == 32767 || val == 73728 || val == 73727 ||
                            val == 576 || val == 512 || val == 575 || val == 511 ||
                            (isPathFindOrRouting && (val == 262144 || val == 262143)))
                        {
                            bool shouldChange = false;

                            for (int j = i + 1; j < Math.Min(i + 3, codes.Count); j++)
                            {
                                var nextOp = codes[j].OpCode;

                                if (IsPreserveOp(nextOp) && !((nextOp == OpCodes.Stloc || nextOp == OpCodes.Stloc_S || nextOp == OpCodes.Stloc_0 || nextOp == OpCodes.Stloc_1 || nextOp == OpCodes.Stloc_2 || nextOp == OpCodes.Stloc_3) && (method.Name == "SimulationStepImpl" || method.Name == "SimulationStep" || method.Name == "CalculatePath" || method.Name == "UpdateData" || method.Name == "Deserialize")))
                                {
                                    shouldChange = false;
                                    break;
                                }

                                if (IsBoundsCheckOp(nextOp) || IsAllocationOp(nextOp) || nextOp == OpCodes.Stfld || nextOp == OpCodes.Stsfld)
                                {
                                    shouldChange = true;
                                    break;
                                }
                            }

                            // Check previous instruction for bitwise flag test pattern: AND, BRFALSE
                            if (shouldChange && i >= 2)
                            {
                                var prevOp = codes[i - 1].OpCode;
                                if (prevOp == OpCodes.Brfalse || prevOp == OpCodes.Brfalse_S ||
                                    prevOp == OpCodes.Brtrue || prevOp == OpCodes.Brtrue_S)
                                {
                                    for (int k = i - 2; k >= Math.Max(0, i - 4); k--)
                                    {
                                        if (codes[k].OpCode == OpCodes.And || codes[k].OpCode == OpCodes.Or)
                                        {
                                            shouldChange = false;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (shouldChange)
                            {
                                int expectedVal = val;
                                if (val == 32768 || val == 36864) expectedVal = 65024;
                                else if (val == 32767 || val == 36863) expectedVal = 65023;
                                else if (val == 73728) expectedVal = 130048;
                                else if (val == 73727) expectedVal = 130047;
                                else if (val == 576 || val == 512) expectedVal = 1016;
                                else if (val == 575 || val == 511) expectedVal = 1015;
                                else if (val == 262144) expectedVal = 524288;
                                else if (val == 262143) expectedVal = 524287;

                                Console.WriteLine(string.Format("  CHANGE at IL_{0:X4}: {1} -> {2} (Next Op: {3})", 
                                    instruction.Offset, val, expectedVal, codes[i + 1].OpCode));
                            }
                            else
                            {
                                Console.WriteLine(string.Format("  SKIP at IL_{0:X4}: {1} (Next Op: {2})", 
                                    instruction.Offset, val, codes[i + 1].OpCode));
                            }
                        }
                    }
                }
            }
        }
    }
}
