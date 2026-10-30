using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        Assembly asm = Assembly.LoadFrom(managed + "Assembly-CSharp.dll");
        Type rmType = asm.GetType("RenderManager");
        MethodInfo m = rmType.GetMethod("GetColorLocation", new Type[] { typeof(uint), typeof(int).MakeByRefType(), typeof(int).MakeByRefType() });
        var body = m.GetMethodBody();
        byte[] il = body.GetILAsByteArray();
        Console.WriteLine($"IL bytes: {BitConverter.ToString(il)}");
    }
}
