using System;
using System.IO;

class Program
{
    static void Main()
    {
        // Check Player.log for all mentions of Deserialize_Patch
        string logPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Logs/Unity/Player.log";
        if (File.Exists(logPath))
        {
            foreach (var line in File.ReadAllLines(logPath))
            {
                if (line.Contains("NetManager+Data.Deserialize_Patch") || line.Contains("NetManager.Data.Deserialize"))
                {
                    Console.WriteLine(line);
                }
            }
        }
    }
}
