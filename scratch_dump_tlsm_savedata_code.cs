using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string dir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/";
        Assembly asm = Assembly.LoadFrom(dir + "TrafficManager.dll");
        Type t = asm.GetType("TrafficManager.Manager.Impl.TrafficLightSimulationManager");
        MethodInfo m = t.GetMethod("SaveData", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        var body = m.GetMethodBody();
        byte[] il = body.GetILAsByteArray();

        // Let's print all instructions with IL offset
        int i = 0;
        while (i < il.Length)
        {
            int offset = i;
            byte op = il[i++];
            if (op == 0x20) // ldc.i4
            {
                int val = BitConverter.ToInt32(il, i);
                i += 4;
            }
            else if (op == 0x72) // ldstr
            {
                int token = BitConverter.ToInt32(il, i);
                i += 4;
            }
            else if (op >= 0x6F && op <= 0x7E) // call, etc
            {
                int token = BitConverter.ToInt32(il, i);
                try {
                    MemberInfo member = m.Module.ResolveMember(token);
                    Console.WriteLine($"IL_{offset:X4}: call/fld {member.DeclaringType.Name}.{member.Name}");
                } catch {}
                i += 4;
            }
        }
    }
}
