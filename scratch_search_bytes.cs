using System;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        string p = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        byte[] fileBytes = File.ReadAllBytes(p);
        Console.WriteLine("File size: " + fileBytes.Length);

        string[] search = { "VehicleManager", "PathManager", "NetManager", "CitizenManager" };
        foreach (var s in search)
        {
            byte[] b = Encoding.UTF8.GetBytes(s);
            int count = 0;
            for (int i = 0; i < fileBytes.Length - b.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < b.Length; j++)
                {
                    if (fileBytes[i + j] != b[j]) { match = false; break; }
                }
                if (match)
                {
                    count++;
                    if (count <= 3) Console.WriteLine("Found '" + s + "' at offset " + i);
                }
            }
            Console.WriteLine("Total matches for '" + s + "': " + count);
        }
    }
}
