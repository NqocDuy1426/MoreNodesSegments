using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string tmpePath = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        Assembly asm = Assembly.LoadFrom(tmpePath);
        Type sde = asm.GetType("TrafficManager.Lifecycle.SerializableDataExtension");
        MethodInfo m = sde.GetMethod("LoadDataState", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

        var body = m.GetMethodBody();
        byte[] il = body.GetILAsByteArray();

        for (int i = 0; i < il.Length - 4; i++)
        {
            int token = BitConverter.ToInt32(il, i);
            try
            {
                MemberInfo member = m.Module.ResolveMember(token);
                if (member.Name.Contains("LoadData") || member.Name.Contains("Manager") || member.Name.Contains("Instance") || member.Name.Contains("TrafficLight") || member.Name.Contains("Segment"))
                {
                    Console.WriteLine($"IL_{i:X4}: Token {token:X8} => {member.DeclaringType.Name}.{member.Name}");
                }
            }
            catch {}
        }
    }
}
