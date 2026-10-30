using System;
using System.Reflection;
using System.Reflection.Emit;

class Program
{
    static void Main()
    {
        string csPath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
        {
            string name = new AssemblyName(args.Name).Name;
            string p = System.IO.Path.Combine(csPath, name + ".dll");
            if (System.IO.File.Exists(p)) return Assembly.LoadFrom(p);
            return null;
        };
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        Assembly tmpe = Assembly.LoadFrom(tmpePath);
        Type t = tmpe.GetType("TrafficManager.Patch._RoadBaseAI.GetTrafficLightNodeStatePatch");
        MethodInfo m = t.GetMethod("Prefix", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        var mb = m.GetMethodBody();
        byte[] il = mb.GetILAsByteArray();
        Module mod = tmpe.ManifestModule;

        FieldInfo[] fields = typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static);
        int pos = 0;
        while (pos < il.Length)
        {
            int offset = pos;
            byte b = il[pos++];
            OpCode op = OpCodes.Nop;
            if (b == 0xfe)
            {
                byte b2 = il[pos++];
                short val = (short)(0xfe00 | b2);
                foreach (var f in fields)
                {
                    if (f.FieldType == typeof(OpCode) && ((OpCode)f.GetValue(null)).Value == val)
                    {
                        op = (OpCode)f.GetValue(null);
                        break;
                    }
                }
            }
            else
            {
                foreach (var f in fields)
                {
                    if (f.FieldType == typeof(OpCode) && ((OpCode)f.GetValue(null)).Value == b)
                    {
                        op = (OpCode)f.GetValue(null);
                        break;
                    }
                }
            }

            string operandStr = "";
            switch (op.OperandType)
            {
                case OperandType.InlineMethod:
                case OperandType.InlineField:
                case OperandType.InlineType:
                case OperandType.InlineTok:
                    int token = BitConverter.ToInt32(il, pos);
                    pos += 4;
                    try
                    {
                        MemberInfo mem = mod.ResolveMember(token);
                        operandStr = mem.DeclaringType.Name + "." + mem.Name;
                    }
                    catch (Exception ex) { operandStr = "token:" + token.ToString("X") + " (" + ex.Message + ")"; }
                    break;
                case OperandType.InlineString:
                    int strToken = BitConverter.ToInt32(il, pos);
                    pos += 4;
                    try { operandStr = "\"" + mod.ResolveString(strToken) + "\""; } catch { operandStr = "str:" + strToken.ToString("X"); }
                    break;
                case OperandType.InlineI:
                    int val = BitConverter.ToInt32(il, pos); pos += 4; operandStr = val.ToString(); break;
                case OperandType.ShortInlineI:
                    sbyte sb = (sbyte)il[pos++]; operandStr = sb.ToString(); break;
                case OperandType.InlineBrTarget:
                    int br = BitConverter.ToInt32(il, pos); pos += 4; operandStr = "IL_" + (pos + br).ToString("X4"); break;
                case OperandType.ShortInlineBrTarget:
                    sbyte sbr = (sbyte)il[pos++]; operandStr = "IL_" + (pos + sbr).ToString("X4"); break;
                case OperandType.ShortInlineVar:
                    operandStr = "V_" + il[pos++]; break;
                default:
                    break;
            }
            Console.WriteLine(string.Format("IL_{0:X4}: {1} {2}", offset, op.Name, operandStr));
        }
    }
}
