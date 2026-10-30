using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        Assembly asm = Assembly.LoadFrom(managed + "Assembly-CSharp.dll");
        Type rmType = asm.GetType("RenderManager");
        FieldInfo fMap = rmType.GetField("m_objectColorMap", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        int token = fMap.MetadataToken;

        foreach (var m in rmType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
        {
            var body = m.GetMethodBody();
            if (body == null) continue;
            byte[] il = body.GetILAsByteArray();
            for (int i = 0; i < il.Length - 4; i++)
            {
                if (BitConverter.ToInt32(il, i) == token)
                {
                    Console.WriteLine($"RenderManager.{m.Name} uses m_objectColorMap at IL_{i:X4}");
                }
            }
        }
    }
}
