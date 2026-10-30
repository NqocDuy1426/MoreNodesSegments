using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        Assembly asm = Assembly.LoadFrom(tmpePath);
        Type t = asm.GetType("TrafficManager.Manager.Impl.JunctionRestrictionsManager");
        MethodInfo m = t.GetMethod("SaveData", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        for (int i = 0x01C0; i < 0x01F0; i++)
        {
            if (i + 4 <= m.GetMethodBody().GetILAsByteArray().Length)
            {
                int token = BitConverter.ToInt32(m.GetMethodBody().GetILAsByteArray(), i);
                try
                {
                    MemberInfo member = m.Module.ResolveMember(token);
                    Console.WriteLine($"IL_{i:X4}: Token {token:X8} => {member.DeclaringType.Name}.{member.Name}");
                }
                catch {}
            }
        }
    }
}
