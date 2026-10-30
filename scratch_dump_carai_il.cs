using System;
using System.Reflection;
using System.Reflection.Emit;

class Program
{
    static void Main()
    {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        Assembly asm = Assembly.LoadFrom(managed + "Assembly-CSharp.dll");
        Type carAiType = asm.GetType("CarAI");
        MethodInfo target = null;
        foreach (var method in carAiType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (method.Name == "SimulationStep" && method.GetParameters().Length == 6)
            {
                target = method;
                break;
            }
        }

        var body = target.GetMethodBody();
        byte[] il = body.GetILAsByteArray();
        int start = 0x0600;
        int end = 0x0660;
        Console.WriteLine($"Length={il.Length}");

        // Print raw hex and resolved tokens around start..end
        for (int i = start; i < end; i++)
        {
            if (i + 4 <= il.Length)
            {
                int token = BitConverter.ToInt32(il, i);
                try
                {
                    MemberInfo member = target.Module.ResolveMember(token);
                    Console.WriteLine($"IL_{i:X4}: Token {token:X8} => {member.DeclaringType.Name}.{member.Name}");
                }
                catch {}
            }
        }
    }
}
