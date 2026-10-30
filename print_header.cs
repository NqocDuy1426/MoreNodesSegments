using System;
using System.IO;

public class PrintHeader
{
    public static void Main()
    {
        string p = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup .crp";
        using (FileStream fs = new FileStream(p, FileMode.Open, FileAccess.Read))
        {
            fs.Seek(657, SeekOrigin.Begin); // Right after SimulationMetaData
            Console.WriteLine("Position 657 bytes:");
            byte[] b = new byte[64];
            fs.Read(b, 0, 64);
            for (int i = 0; i < 64; i++)
            {
                Console.Write(b[i].ToString("X2") + " ");
                if ((i + 1) % 16 == 0) Console.WriteLine();
            }
        }
    }
}
