using System;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll");
        var t = asm.MainModule.GetType("TrafficManager.UI.SubTools.SpeedLimitsTool");
        if (t != null)
        {
            Console.WriteLine("Type: " + t.FullName);
            foreach (var m in t.Methods)
            {
                if (m.Name.Contains("Show") || m.Name.Contains("Select") || m.Name.Contains("Click") || m.Name.Contains("Primary") || m.Name.Contains("Segment"))
                {
                    Console.WriteLine("  Method: " + m.FullName);
                }
            }
        }
    }
}
