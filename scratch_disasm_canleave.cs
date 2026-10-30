using System;
using System.Reflection;
using System.Reflection.Emit;

class Program
{
    static void Main()
    {
        var asm = Assembly.LoadFrom("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        
        DumpMethod(asm.GetType("PassengerCarAI").GetMethod("CanLeave", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
        DumpMethod(asm.GetType("BusAI").GetMethod("CanLeave", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
    }

    static void DumpMethod(MethodInfo m)
    {
        if (m == null) return;
        Console.WriteLine("\n=== " + m.DeclaringType.Name + "." + m.Name + " ===");
        var body = m.GetMethodBody();
        if (body == null) { Console.WriteLine("No body"); return; }
        var bytes = body.GetILAsByteArray();
        Console.WriteLine("IL Length: " + bytes.Length);
        
        // Print opcodes
        int i = 0;
        while (i < bytes.Length)
        {
            int offset = i;
            byte b = bytes[i++];
            OpCode op;
            if (b == 0xfe)
            {
                byte b2 = bytes[i++];
                op = typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static)[0].GetValue(null) as OpCode? ?? OpCodes.Nop;
                Console.WriteLine("  IL_{0:X4}: FE {1:X2}", offset, b2);
            }
            else
            {
                Console.WriteLine("  IL_{0:X4}: {1:X2}", offset, b);
            }
        }
    }
}
