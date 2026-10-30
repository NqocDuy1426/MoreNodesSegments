using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll";
        var assembly = AssemblyDefinition.ReadAssembly(path);
        
        foreach (var t in assembly.MainModule.Types)
        {
            foreach (var m in t.Methods)
            {
                if (m.Body != null)
                {
                    foreach (var ins in m.Body.Instructions)
                    {
                        if (ins.OpCode == OpCodes.Call || ins.OpCode == OpCodes.Callvirt)
                        {
                            var target = ins.Operand as MethodReference;
                            if (target != null && target.Name == "TrafficLightSimulationStep")
                            {
                                Console.WriteLine(t.FullName + "::" + m.Name + " -> " + target.ToString());
                            }
                        }
                    }
                }
            }
        }
    }
}
