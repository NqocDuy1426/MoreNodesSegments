using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var vm = asm.MainModule.GetType("VehicleManager");
        foreach (var nested in vm.NestedTypes)
        {
            if (nested.Name == "Data")
            {
                foreach (var m in nested.Methods)
                {
                    if (m.Name == "Deserialize")
                    {
                        for (int i = 0; i < m.Body.Instructions.Count; i++)
                        {
                            var inst = m.Body.Instructions[i];
                            if (inst.Offset >= 0x0060 && inst.Offset <= 0x0120)
                            {
                                Console.WriteLine("  {0:X4}: {1} {2}", inst.Offset, inst.OpCode, inst.Operand);
                            }
                        }
                    }
                }
            }
        }
    }
}
