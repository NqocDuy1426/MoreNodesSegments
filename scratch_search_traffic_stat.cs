using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        Assembly asm = Assembly.LoadFrom(managed + "Assembly-CSharp.dll");
        Type vmType = asm.GetType("VehicleManager");
        int totalToken = vmType.GetField("m_totalTrafficFlow").MetadataToken;
        int maxToken = vmType.GetField("m_maxTrafficFlow").MetadataToken;
        int lastToken = vmType.GetField("m_lastTrafficFlow").MetadataToken;

        Console.WriteLine($"totalToken: {totalToken:X8}, maxToken: {maxToken:X8}, lastToken: {lastToken:X8}");

        foreach (Type t in asm.GetTypes())
        {
            foreach (MethodInfo m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                var body = m.GetMethodBody();
                if (body == null) continue;
                byte[] il = body.GetILAsByteArray();
                for (int i = 0; i < il.Length - 4; i++)
                {
                    int token = BitConverter.ToInt32(il, i);
                    if (token == totalToken || token == maxToken || token == lastToken)
                    {
                        Console.WriteLine($"{t.FullName}.{m.Name} touches token {token:X8} at IL_{i:X4}");
                    }
                }
            }
        }
    }
}
