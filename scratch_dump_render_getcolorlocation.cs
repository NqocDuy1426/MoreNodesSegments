using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        Assembly asm = Assembly.LoadFrom(managed + "Assembly-CSharp.dll");
        Type rmType = asm.GetType("RenderManager");
        MethodInfo m = rmType.GetMethod("GetColorLocation", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        if (m == null)
        {
            Console.WriteLine("GetColorLocation not found");
            return;
        }

        var body = m.GetMethodBody();
        byte[] il = body.GetILAsByteArray();
        Console.WriteLine($"GetColorLocation length: {il.Length}");

        // Print instructions or disassembly
    }
}
