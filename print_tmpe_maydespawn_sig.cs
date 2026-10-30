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
        var meth = type.Methods.FirstOrDefault(method => method.Name == "MayDespawn");
        Console.WriteLine("MayDespawn isStatic: " + meth.IsStatic + ", isPublic: " + meth.IsPublic);
        foreach (var p in meth.Parameters)
        {
            Console.WriteLine("  Param: " + p.ParameterType.FullName + " " + p.Name);
        }
        Console.WriteLine("ReturnType: " + meth.ReturnType.FullName);
    }
}
