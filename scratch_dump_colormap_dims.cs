using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        Assembly asm = Assembly.LoadFrom(managed + "Assembly-CSharp.dll");
        Type rmType = asm.GetType("RenderManager");
        FieldInfo fw = rmType.GetField("OBJECT_COLORMAP_WIDTH", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        FieldInfo fh = rmType.GetField("OBJECT_COLORMAP_HEIGHT", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        Console.WriteLine($"WIDTH = {fw.GetValue(null)}, HEIGHT = {fh.GetValue(null)}");
    }
}
