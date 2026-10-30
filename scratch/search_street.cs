using System;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        string savePath = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/backup4.crp";
        byte[] bytes = File.ReadAllBytes(savePath);
        Console.WriteLine("Save size: " + bytes.Length);

        // Search for "Hương Giang"
        byte[] search = Encoding.UTF8.GetBytes("Hương Giang");
        for (int i = 0; i < bytes.Length - search.Length; i++)
        {
            bool match = true;
            for (int j = 0; j < search.Length; j++)
            {
                if (bytes[i + j] != search[j]) { match = false; break; }
            }
            if (match)
            {
                Console.WriteLine("Found 'Hương Giang' at offset " + i);
            }
        }
    }
}
