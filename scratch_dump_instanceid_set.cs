using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        string managed = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/";
        Assembly asm = Assembly.LoadFrom(managed + "Assembly-CSharp.dll");
        Type idType = asm.GetType("InstanceID");
        MethodInfo mNode = idType.GetMethod("set_NetNode");
        MethodInfo mSeg = idType.GetMethod("set_NetSegment");

        Console.WriteLine($"set_NetNode bytes: {BitConverter.ToString(mNode.GetMethodBody().GetILAsByteArray())}");
        Console.WriteLine($"set_NetSegment bytes: {BitConverter.ToString(mSeg.GetMethodBody().GetILAsByteArray())}");
    }
}
