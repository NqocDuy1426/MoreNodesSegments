using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using HarmonyLib;
using Mono.Cecil;

class SimTranspiledIL
{
    static void Main()
    {
        var asm = Assembly.LoadFrom("content/MoreNetworkCapacity.Core.dll");
        var patchType = asm.GetType("MoreNetworkCapacity.Patches.NetManagerDeserializePatch");
        var transpilerMethod = patchType.GetMethod("Transpiler", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

        var gameAsm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var netMgr = gameAsm.MainModule.Types.First(t => t.Name == "NetManager");
        var netMgrData = netMgr.NestedTypes.First(t => t.Name == "Data");
        var desMethod = netMgrData.Methods.First(m => m.Name == "Deserialize");

        var instructions = new List<CodeInstruction>();
        foreach (var inst in desMethod.Body.Instructions)
        {
            var opField = typeof(System.Reflection.Emit.OpCodes).GetField(inst.OpCode.Code.ToString(), BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
            if (opField != null)
            {
                var op = (System.Reflection.Emit.OpCode)opField.GetValue(null);
                instructions.Add(new CodeInstruction(op, inst.Operand));
            }
        }

        var result = (IEnumerable<CodeInstruction>)transpilerMethod.Invoke(null, new object[] { instructions });
        var list = new List<CodeInstruction>(result);
        Console.WriteLine("Transpiled instructions count: " + list.Count);
        for (int i = 0; i < 40; i++)
        {
            Console.WriteLine(string.Format("  [{0}] {1} {2}", i, list[i].opcode, list[i].operand));
        }
    }
}
