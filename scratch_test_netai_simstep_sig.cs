using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        var gameDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = Assembly.LoadFrom(gameDir + "/Assembly-CSharp.dll");

        Type netAI = asm.GetType("NetAI");
        Type netSeg = asm.GetType("NetSegment");
        Type netNode = asm.GetType("NetNode");

        MethodInfo mSeg = netAI.GetMethod("SimulationStep", new Type[] { typeof(ushort), netSeg.MakeByRefType() });
        Console.WriteLine($"NetAI.SimulationStep(ushort, ref NetSegment): {mSeg != null}");

        MethodInfo mNode = netAI.GetMethod("SimulationStep", new Type[] { typeof(ushort), netNode.MakeByRefType() });
        Console.WriteLine($"NetAI.SimulationStep(ushort, ref NetNode): {mNode != null}");
    }
}
