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
        foreach (var type in assembly.Modules.SelectMany(m => m.GetTypes()))
        {
            foreach (var meth in type.Methods.Where(m => m.HasBody))
            {
                foreach (var instr in meth.Body.Instructions)
                {
                    if (instr.OpCode == OpCodes.Stfld && instr.Operand?.ToString().Contains("segmentId") == true && instr.Operand?.ToString().Contains("ExtSegment") == true)
                    {
                        Console.WriteLine(type.FullName + "." + meth.Name + " sets ExtSegment.segmentId");
                    }
                }
            }
        }
    }
}
