using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        Assembly asm = Assembly.LoadFrom(managed + "Assembly-CSharp.dll");
        Type rmType = asm.GetType("RenderManager");
        int token1 = rmType.GetMethod("GetColorLocation", new Type[] { typeof(uint) }).MetadataToken;
        int token2 = rmType.GetMethod("GetColorLocation", new Type[] { typeof(uint), typeof(int).MakeByRefType(), typeof(int).MakeByRefType() }).MetadataToken;

        foreach (var t in asm.GetTypes())
        {
            foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
            {
                var body = m.GetMethodBody();
                if (body == null) continue;
                byte[] il = body.GetILAsByteArray();
                for (int i = 0; i < il.Length - 4; i++)
                {
                    int token = BitConverter.ToInt32(il, i);
                    if (token == token1 || token == token2)
                    {
                        Console.WriteLine($"{t.FullName}.{m.Name} calls GetColorLocation at IL_{i:X4}");
                    }
                }
            }
        }
    }
}
