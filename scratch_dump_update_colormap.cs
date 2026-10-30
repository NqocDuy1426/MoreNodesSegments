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
        Console.WriteLine($"UpdateColorMap IL length: {il.Length}");

        // Print tokens in UpdateColorMap
        for (int i = 0; i < il.Length - 4; i++)
        {
            int token = BitConverter.ToInt32(il, i);
            try
            {
                MemberInfo member = target.Module.ResolveMember(token);
                if (member.Name.Contains("m_color") || member.Name.Contains("Segment") || member.Name.Contains("Node") || member.Name.Contains("Color") || member.Name.Contains("SetPixel"))
                {
                    Console.WriteLine($"IL_{i:X4}: Token {token:X8} => {member.DeclaringType.Name}.{member.Name}");
                }
            }
            catch {}
        }
    }
}
