using System;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        string tmpePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var assembly = AssemblyDefinition.ReadAssembly(tmpePath);
        var type = assembly.Modules.SelectMany(mod => mod.Types).FirstOrDefault(t => t.FullName == "TrafficManager.Manager.Impl.VehicleBehaviorManager");
        foreach (var meth in type.Methods.Where(m => m.Name == "MayChangeSegment"))
        {
            Console.WriteLine(meth.ReturnType + " " + meth.Name + "(" + string.Join(", ", meth.Parameters.Select(p => p.ParameterType.ToString() + " " + p.Name).ToArray()) + ")");
        }
    }
}
