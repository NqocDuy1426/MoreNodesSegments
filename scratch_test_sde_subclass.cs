using System;
using ICities;

public class TestSDE : SerializableDataExtensionBase
{
    public void Test()
    {
        if (serializableDataManager != null)
        {
            serializableDataManager.SaveData("test", new byte[0]);
        }
    }
}

class Program
{
    static void Main()
    {
        TestSDE t = new TestSDE();
        Console.WriteLine("Compiled and instantiated successfully!");
    }
}
