using System;
using System.IO;
using System.Reflection;

class Program
{
    static void Main()
    {
        string gameDir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        Assembly asm = Assembly.LoadFrom(Path.Combine(gameDir, "Assembly-CSharp.dll"));
        Assembly.LoadFrom(Path.Combine(gameDir, "ICities.dll"));

        Type sdw = asm.GetType("SerializableDataWrapper");
        MethodInfo m = sdw.GetMethod("LoadData", new Type[] { typeof(string) });
        Console.WriteLine("SerializableDataWrapper.LoadData method: " + (m != null));
    }
}
