using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        Assembly asm = Assembly.LoadFrom(managed + "Assembly-CSharp.dll");
        Type roadAi = asm.GetType("RoadBaseAI");
        MethodInfo target = null;
        foreach (var m in roadAi.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (m.Name == "GetColor" && m.GetParameters().Length == 4 && m.GetParameters()[1].ParameterType.Name.Contains("NetSegment"))
            {
                target = m;
                break;
            }
        }

        var body = target.GetMethodBody();
        byte[] il = body.GetILAsByteArray();
        Console.WriteLine($"GetColor IL length: {il.Length}");

        // Print resolved tokens
        for (int i = 0; i < il.Length - 4; i++)
        {
            int token = BitConverter.ToInt32(il, i);
            try
            {
                MemberInfo member = target.Module.ResolveMember(token);
                if (member.Name.Contains("traffic") || member.Name.Contains("Traffic") || member.Name.Contains("Color") || member.Name.Contains("Lerp"))
                {
                    Console.WriteLine($"IL_{i:X4}: Token {token:X8} => {member.DeclaringType.Name}.{member.Name}");
                }
            }
            catch {}
        }
    }
}
