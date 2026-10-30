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
        Console.WriteLine($"Save IL length: {il.Length}");

        // Print called methods in Save
        for (int i = 0; i < il.Length - 4; i++)
        {
            int token = BitConverter.ToInt32(il, i);
            try
            {
                MemberInfo member = mSave.Module.ResolveMember(token);
                if (member.Name.Contains("Save") || member.Name.Contains("Serialize") || member.Name.Contains("Manager"))
                {
                    Console.WriteLine($"IL_{i:X4}: Token {token:X8} => {member.DeclaringType.Name}.{member.Name}");
                }
            }
            catch {}
        }
    }
}
