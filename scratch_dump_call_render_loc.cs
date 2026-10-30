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

        // Let's dump bytes around 0x01A0 to 0x01C0
        for (int i = 0x01A0; i <= 0x01B8; i++)
        {
            Console.Write($"{il[i]:X2} ");
        }
        Console.WriteLine();
    }
}
