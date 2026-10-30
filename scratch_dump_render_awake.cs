using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        Assembly asm = Assembly.LoadFrom(managed + "Assembly-CSharp.dll");
        Type rmType = asm.GetType("RenderManager");
        MethodInfo m = rmType.GetMethod("Awake", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        var body = m.GetMethodBody();
        byte[] il = body.GetILAsByteArray();

        // Print around IL_00D0 to IL_0120
        for (int i = 0x00C0; i < 0x0120; i++)
        {
            if (i + 4 <= il.Length)
            {
                int token = BitConverter.ToInt32(il, i);
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
