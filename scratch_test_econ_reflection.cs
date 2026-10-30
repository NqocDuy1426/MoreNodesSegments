using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        var gameDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
            "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        var asm = Assembly.LoadFrom(gameDir + "/Assembly-CSharp.dll");

        Type em = asm.GetType("EconomyManager");
        Type resType = asm.GetType("EconomyManager+Resource");
        Type servType = asm.GetType("ItemClass+Service");
        Type subType = asm.GetType("ItemClass+SubService");
        Type levType = asm.GetType("ItemClass+Level");
        Type taxType = asm.GetType("DistrictPolicies+Taxation");

        MethodInfo mFetch = em.GetMethod("FetchResource", BindingFlags.Public | BindingFlags.Instance, null,
            new Type[] { resType, typeof(int), servType, subType, levType }, null);
        Console.WriteLine($"FetchResource found: {mFetch != null}");

        MethodInfo mAdd = em.GetMethod("AddResource", BindingFlags.Public | BindingFlags.Instance, null,
            new Type[] { resType, typeof(int), servType, subType, levType, taxType }, null);
        Console.WriteLine($"AddResource found: {mAdd != null}");

        MethodInfo mPriv = em.GetMethod("AddPrivateIncome", BindingFlags.Public | BindingFlags.Instance, null,
            new Type[] { typeof(int), servType, subType, levType, typeof(int) }, null);
        Console.WriteLine($"AddPrivateIncome found: {mPriv != null}");

        Type pNet = asm.GetType("PlayerNetAI");
        Type v3 = typeof(UnityEngine.Vector3);
        MethodInfo mMaint = pNet.GetMethod("GetMaintenanceCost", BindingFlags.Public | BindingFlags.Instance, null,
            new Type[] { v3, v3 }, null);
        Console.WriteLine($"PlayerNetAI.GetMaintenanceCost found: {mMaint != null}");
    }
}
