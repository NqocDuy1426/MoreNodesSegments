using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        string savePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        if (!File.Exists(savePath)) { Console.WriteLine("Save not found"); return; }
        byte[] bytes = File.ReadAllBytes(savePath);
        Console.WriteLine("Save file size: " + bytes.Length);

        // Search for "VehicleManager" in save
        byte[] vmPattern = System.Text.Encoding.UTF8.GetBytes("VehicleManager");
        for (int i = 0; i < bytes.Length - vmPattern.Length; i++)
        {
            bool match = true;
            for (int j = 0; j < vmPattern.Length; j++)
            {
                if (bytes[i + j] != vmPattern[j]) { match = false; break; }
            }
            if (match)
            {
                Console.WriteLine("Found VehicleManager at offset: " + i);
            }
        }
    }
}
