using System;
using System.Reflection;
using System.Reflection.Emit;

class Program {
    static void Main() {
        var asm = Assembly.LoadFrom("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll");
        var lp = asm.GetType("LoadPanel");
        var m = lp.GetMethod("Refresh", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        var mb = m.GetMethodBody();
        byte[] il = mb.GetILAsByteArray();
        var module = m.Module;
        
        int pos = 0;
        while (pos < il.Length) {
            int offset = pos;
            byte op = il[pos++];
            OpCode opcode = OpCodes.Nop;
            if (op == 0xFE) {
                byte op2 = il[pos++];
                foreach (var f in typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static)) {
                    var c = (OpCode)f.GetValue(null);
                    if (c.Size == 2 && c.Value == unchecked((short)((op << 8) | op2))) {
                        opcode = c;
                        break;
                    }
                }
            } else {
                foreach (var f in typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static)) {
                    var c = (OpCode)f.GetValue(null);
                    if (c.Size == 1 && c.Value == op) {
                        opcode = c;
                        break;
                    }
                }
            }
            
            string operandStr = "";
            switch (opcode.OperandType) {
                case OperandType.InlineNone:
                    break;
                case OperandType.InlineI:
                case OperandType.InlineBrTarget:
                    int val = BitConverter.ToInt32(il, pos);
                    pos += 4;
                    operandStr = (opcode.OperandType == OperandType.InlineBrTarget) ? $"IL_{offset + 1 + 4 + val:x4}" : val.ToString();
                    break;
                case OperandType.ShortInlineI:
                case OperandType.ShortInlineBrTarget:
                    sbyte sval = (sbyte)il[pos++];
                    operandStr = (opcode.OperandType == OperandType.ShortInlineBrTarget) ? $"IL_{offset + 1 + 1 + sval:x4}" : sval.ToString();
                    break;
                case OperandType.InlineMethod:
                case OperandType.InlineField:
                case OperandType.InlineType:
                case OperandType.InlineTok:
                    int tok = BitConverter.ToInt32(il, pos);
                    pos += 4;
                    try {
                        var member = module.ResolveMember(tok);
                        operandStr = $"{member.DeclaringType.Name}::{member.Name}";
                    } catch {
                        operandStr = $"0x{tok:X8}";
                    }
                    break;
                case OperandType.InlineString:
                    int stok = BitConverter.ToInt32(il, pos);
                    pos += 4;
                    try {
                        operandStr = $"\"{module.ResolveString(stok)}\"";
                    } catch {
                        operandStr = $"str 0x{stok:X8}";
                    }
                    break;
                default:
                    operandStr = opcode.OperandType.ToString();
                    break;
            }
            if (opcode == OpCodes.Call || opcode == OpCodes.Callvirt || opcode.Name.StartsWith("ldstr"))
                Console.WriteLine($"IL_{offset:x4}: {opcode.Name} {operandStr}");
        }
    }
}
