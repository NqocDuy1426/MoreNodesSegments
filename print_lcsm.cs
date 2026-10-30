using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string tmpePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        var assembly = AssemblyDefinition.ReadAssembly(tmpePath);
        var type = assembly.Modules.SelectMany(m => m.GetTypes()).FirstOrDefault(t => t.FullName == "TrafficManager.Manager.Impl.LaneConnection.LaneConnectionSubManager");
        if (type != null)
        {
            Console.WriteLine("Type: " + type.FullName);
            foreach (var meth in type.Methods)
            {
                Console.WriteLine("\n--- Method: " + meth.Name + " (" + meth.Parameters.Count + " params) returns " + meth.ReturnType + " ---");
                if (meth.HasBody)
                {
                    for (int i = 0; i < meth.Body.Instructions.Count; i++)
                    {
                        var instr = meth.Body.Instructions[i];
                        Console.WriteLine(string.Format("IL_{0:X4}: {1} {2}", instr.Offset, instr.OpCode, instr.Operand));
                    }
                }
            }
        }
    }
}
