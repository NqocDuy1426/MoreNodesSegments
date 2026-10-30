using System;
using System.IO;

class Program
{
    static void Main()
    {
        string logPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Logs/Unity/Player.log";
        var lines = File.ReadAllLines(logPath);
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("'More Vehicles' has been disabled"))
            {
                for (int j = Math.Max(0, i - 15); j <= Math.Min(lines.Length - 1, i + 15); j++)
                {
                    Console.WriteLine(lines[j]);
                }
                break;
            }
        }
    }
}
