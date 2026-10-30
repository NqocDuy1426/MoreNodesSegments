using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string tmpePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var assembly = AssemblyDefinition.ReadAssembly(tmpePath);
        foreach (var type in assembly.Modules.SelectMany(m => m.Types))
        {
            foreach (var meth in type.Methods.Where(m => m.HasBody))
            {
                foreach (var instr in meth.Body.Instructions)
                {
                    if (instr.Operand is MethodReference mr && mr.Name == "MayChangeSegment")
                    {
                        Console.WriteLine(type.FullName + "." + meth.Name + " calls " + mr.ToString());
                    }
                }
            }
        }
    }
}
