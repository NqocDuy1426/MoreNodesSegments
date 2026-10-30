using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program
{
    static void Main()
    {
        var gameDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = AssemblyDefinition.ReadAssembly(gameDir + "/Assembly-CSharp.dll");

        // ItemClass.Service.PublicTransport = 14
        // ItemClass.SubService.PublicTransportTrain = 11
        Console.WriteLine("Searching for PublicTransport + Train or TrainTrackAI or TrainStationAI or TransportLine...");
        
        foreach (var type in asm.MainModule.Types)
        {
            if (type.Name.Contains("Train") || type.Name.Contains("TransportLine"))
            {
                foreach (var m in type.Methods)
                {
                    if (m.Body == null) continue;
                    foreach (var inst in m.Body.Instructions)
                    {
                        if (inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt)
                        {
                            var mr = inst.Operand as MethodReference;
                            if (mr != null && mr.DeclaringType.Name == "EconomyManager")
                            {
                                Console.WriteLine($"{type.FullName}.{m.Name} calls {mr.Name} at IL_{inst.Offset:X4}");
                            }
                        }
                    }
                }
            }
        }
    }
}
