using System;
using System.Reflection;

class Program {
    static void Main() {
        var asm = Assembly.LoadFrom("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var t = asm.GetType("CombinedMilestone+CombinedData");
        Console.WriteLine("Type: " + t);
        foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
            Console.WriteLine("Field: " + f.Name + " (" + f.FieldType + ")");
        }
    }
}
