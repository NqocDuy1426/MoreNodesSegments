using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        Assembly asm = Assembly.LoadFrom(managed + "Assembly-CSharp.dll");
        Type nmType = asm.GetType("NetManager");
        MethodInfo target = nmType.GetMethod("UpdateColorMap", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(UnityEngine.Texture2D) }, null);
        var body = target.GetMethodBody();
        byte[] il = body.GetILAsByteArray();

        // Let's dump IL from IL_0180 to IL_0220
        for (int i = 0x0180; i < 0x0220; i++)
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
