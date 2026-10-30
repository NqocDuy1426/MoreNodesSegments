using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
        var m = asm.MainModule.GetType("NetManager/Data").Methods.First(x => x.Name == "Deserialize");
        
        // Find which local corresponds to which array:
        // loc.0: NetManager instance
        // loc.1: m_lanes.m_buffer
        // loc.2: m_nodes.m_buffer
        // loc.3: m_segments.m_buffer
        // V_4: m_nodeGrid
        // V_5: m_segmentGrid
        
        for (int i = 0; i < m.Body.Instructions.Count; i++) {
            var instr = m.Body.Instructions[i];
            if (IsArrayOp(instr.OpCode.Code)) {
                // trace back to find what array was loaded
                string arrayDesc = "unknown";
                string indexDesc = "unknown";
                for (int j = i - 1; j >= Math.Max(0, i - 10); j--) {
                    var prev = m.Body.Instructions[j];
                    if (prev.OpCode.Code == Code.Ldloc_1) arrayDesc = "loc.1 (lanes)";
                    else if (prev.OpCode.Code == Code.Ldloc_2) arrayDesc = "loc.2 (nodes)";
                    else if (prev.OpCode.Code == Code.Ldloc_3) arrayDesc = "loc.3 (segments)";
                    else if (prev.OpCode.Code == Code.Ldfld && prev.Operand != null) arrayDesc = prev.Operand.ToString();
                }
                var prev1 = m.Body.Instructions[i - 1];
                indexDesc = $"{prev1.OpCode} {prev1.Operand}";
                Console.WriteLine($"IL_{instr.Offset:X4} {instr.OpCode}: Array={arrayDesc}, Index={indexDesc}");
            }
        }
    }

    static bool IsArrayOp(Code code) {
        return code == Code.Ldelem_I1 || code == Code.Ldelem_I2 || code == Code.Ldelem_I4 || code == Code.Ldelem_I8 ||
               code == Code.Ldelem_U1 || code == Code.Ldelem_U2 || code == Code.Ldelem_U4 || code == Code.Ldelem_R4 ||
               code == Code.Ldelem_R8 || code == Code.Ldelem_Ref || code == Code.Ldelem_Any || code == Code.Ldelema ||
               code == Code.Stelem_I1 || code == Code.Stelem_I2 || code == Code.Stelem_I4 || code == Code.Stelem_I8 ||
               code == Code.Stelem_R4 || code == Code.Stelem_R8 || code == Code.Stelem_Ref || code == Code.Stelem_Any;
    }
}
