using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string gamePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var asm = AssemblyDefinition.ReadAssembly(gamePath);

        var vmType = asm.Modules.SelectMany(m => m.Types).FirstOrDefault(t => t.FullName == "VehicleManager");
        var dataType = vmType.NestedTypes.FirstOrDefault(t => t.Name == "Data");
        if (dataType != null)
        {
            Console.WriteLine("NestedType: " + dataType.FullName);
            foreach (var m in dataType.Methods)
            {
                Console.WriteLine("  " + m.Name);
            }

            var des = dataType.Methods.FirstOrDefault(m => m.Name == "Deserialize");
            if (des != null)
            {
                Console.WriteLine("\n--- VehicleManager.Data.Deserialize IL ---");
                foreach (var ins in des.Body.Instructions)
                {
                    if (ins.Operand != null && (ins.Operand.ToString().Contains("Vehicle") || ins.Operand.ToString().Contains("Read") || ins.Operand.ToString().Contains("buffer") || ins.Operand.ToString().Contains("16384") || ins.Operand.ToString().Contains("65536")))
                    {
                        Console.WriteLine("  " + ins.Offset.ToString("X4") + ": " + ins.OpCode + " " + ins.Operand);
                    }
                }
            }
        }
    }
}
