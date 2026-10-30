using System;
using System.IO;

class Program
{
    static void Main()
    {
        string savePath = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        var fi = new FileInfo(savePath);
        Console.WriteLine($"Save size: {fi.Length} bytes, modified: {fi.LastWriteTime}");
    }
}
