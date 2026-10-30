using System;
using System.Linq;
using Mono.Cecil;

class Program
{
    static void Main()
    {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/CS_MoreNetworkCapacity_Renew/Assembly-CSharp.dll");
        var net = asm.MainModule.GetType("NetManager");
        var data = net.NestedTypes.First(t => t.Name == "Data");
        Console.WriteLine("NetManager.Data fields:");
        foreach (var f in data.Fields)
            Console.WriteLine("  " + f.Name + " : " + f.FieldType);
    }
}
