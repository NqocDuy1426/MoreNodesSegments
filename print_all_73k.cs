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
                for (int i = 0; i < meth.Body.Instructions.Count; i++)
                {
                    var instr = meth.Body.Instructions[i];
                    if (instr.OpCode == OpCodes.Ldc_I4 && Convert.ToInt32(instr.Operand) == 73728)
                    {
                        Console.WriteLine("73728 found in " + type.FullName + "." + meth.Name);
                    }
                }
            }
        }
    }
}
