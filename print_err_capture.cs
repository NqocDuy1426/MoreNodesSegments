using System;
using System.Linq;
using Mono.Cecil;

class PrintErrCapture
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("content/MoreNetworkCapacity.Core.dll");
        var er = asm.MainModule.Types.FirstOrDefault(t => t.FullName.Contains("ErrorReport"));
        var m = er.Methods.FirstOrDefault(x => x.Name == "Capture");
        Console.WriteLine("ErrorReport.Capture instructions: " + m.Body.Instructions.Count);
        foreach (var inst in m.Body.Instructions)
        {
            if (inst.OpCode.ToString() == "ldstr" || inst.OpCode.ToString().StartsWith("call"))
                Console.WriteLine(string.Format("  {0}: {1} {2}", inst.Offset.ToString("X4"), inst.OpCode, inst.Operand));
        }
    }
}
