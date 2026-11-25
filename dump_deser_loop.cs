using System;
using System.Reflection;
using System.Text;
using Harmony;

class Program
{
    static void Main()
    {
        Assembly asm = Assembly.LoadFrom("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        Type t = asm.GetType("NetManager+Data");
        MethodInfo m = t.GetMethod("Deserialize", BindingFlags.Public | BindingFlags.Instance);
        var mb = m.GetMethodBody();
        Console.WriteLine("Locals count: " + mb.LocalVariables.Count);
        foreach (var l in mb.LocalVariables)
        {
            Console.WriteLine(string.Format("Local {0}: {1}", l.LocalIndex, l.LocalType));
        }
    }
}
