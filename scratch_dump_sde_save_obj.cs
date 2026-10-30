using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        Assembly asm = Assembly.LoadFrom(tmpePath);
        Type sde = asm.GetType("TrafficManager.Lifecycle.SerializableDataExtension");
        MethodInfo mSave = sde.GetMethod("Save", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

        var body = mSave.GetMethodBody();
        byte[] il = body.GetILAsByteArray();

        // Print around IL_01B4 (where BinaryFormatter.Serialize is called)
        for (int i = 0x0180; i < 0x0200; i++)
        {
            if (i + 4 <= il.Length)
            {
                int token = BitConverter.ToInt32(il, i);
                try
                {
                    MemberInfo member = mSave.Module.ResolveMember(token);
                    Console.WriteLine($"IL_{i:X4}: Token {token:X8} => {member.DeclaringType.Name}.{member.Name}");
                }
                catch {}
            }
        }
    }
}
