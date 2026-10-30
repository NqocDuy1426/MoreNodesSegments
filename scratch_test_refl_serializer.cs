using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

class Program
{
    static void Main()
    {
        string dir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/";
        Assembly tmpe = Assembly.LoadFrom(dir + "TrafficManager.dll");
        Assembly api = Assembly.LoadFrom(dir + "TMPE.API.dll");
        Assembly commons = Assembly.LoadFrom(dir + "CSUtil.Commons.dll");

        Type tbType = commons.GetType("CSUtil.Commons.TernaryBool");
        Type ptType = api.GetType("TrafficManager.API.Traffic.Enums.PriorityType");

        Console.WriteLine($"tbType: {tbType.FullName}");
        Console.WriteLine($"ptType: {ptType.FullName}");

        // Convert enum byte to object
        object tbTrue = Enum.ToObject(tbType, (byte)2);
        object ptStop = Enum.ToObject(ptType, (byte)2);

        Console.WriteLine($"tbTrue: {tbTrue}, ptStop: {ptStop}");
    }
}
