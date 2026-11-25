
using System;
using System.IO;
using System.Reflection;
using ColossalFramework;
using ColossalFramework.IO;

class Program
{
    static void Main()
    {
        try
        {
            string savePath = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/backup12.crp";
            if (!File.Exists(savePath)) {
                Console.WriteLine("Save file not found at " + savePath);
                return;
            }
            Console.WriteLine("Found save file: " + savePath);
            
            // Look for package / stream
            using (var fs = File.OpenRead(savePath))
            {
                Console.WriteLine("File length: " + fs.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex);
        }
    }
}
