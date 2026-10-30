using System;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string tmpePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var assembly = AssemblyDefinition.ReadAssembly(tmpePath);

        Console.WriteLine("Subscribers to ReleasingSegment or ReleasedSegment:");
        foreach (var t in assembly.Modules.SelectMany(m => m.Types))
        {
            foreach (var m in t.Methods)
            {
                if (!m.HasBody) continue;
                foreach (var i in m.Body.Instructions)
                {
                    if (i.Operand != null)
                    {
                        string opStr = i.Operand.ToString();
                        if (opStr.Contains("add_ReleasingSegment") || opStr.Contains("add_ReleasedSegment"))
                        {
                            Console.WriteLine(t.FullName + "::" + m.Name + " registers -> " + opStr);
                        }
                    }
                }
            }
        }
    }
}
