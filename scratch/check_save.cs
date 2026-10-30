using System;
using System.IO;

class Program
{
    static void Main()
    {
        string savePath = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/backup4.crp";
        Console.WriteLine("Save exists: " + File.Exists(savePath));
    }
}
