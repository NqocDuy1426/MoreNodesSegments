using System;
using System.Reflection;
using System.Reflection.Emit;

class Program
{
    static void Main()
    {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        Assembly asm = Assembly.LoadFrom(managed + "Assembly-CSharp.dll");
        Type vmType = asm.GetType("VehicleManager");
        FieldInfo fTotal = vmType.GetField("m_totalTrafficFlow");

        foreach (Type t in asm.GetTypes())
        {
            foreach (MethodInfo m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
            {
                if (m.GetMethodBody() == null) continue;
                foreach (var instr in m.GetInstructions())
                {
                    if (instr.Operand is FieldInfo fi && fi.Name == "m_totalTrafficFlow")
                    {
                        Console.WriteLine($"{t.FullName}.{m.Name} accesses m_totalTrafficFlow");
                    }
                }
            }
        }
    }
}
public static class MethodBodyReader
{
    public static System.Collections.Generic.List<Instruction> GetInstructions(this MethodInfo method)
    {
        var list = new System.Collections.Generic.List<Instruction>();
        var body = method.GetMethodBody();
        if (body == null) return list;
        var bytes = body.GetILAsByteArray();
        var module = method.Module;
        int i = 0;
        while (i < bytes.Length)
        {
            byte op = bytes[i++];
            OpCode opcode;
            if (op == 0xfe) opcode = (OpCode)typeof(OpCodes).GetFields()[bytes[i++]].GetValue(null);
            else opcode = (OpCode)typeof(OpCodes).GetFields()[op].GetValue(null);
            // approximate
        }
        return list;
    }
    public class Instruction { public object Operand; }
}
