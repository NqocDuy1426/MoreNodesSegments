using System;
using System.IO;
using System.Collections.Generic;

public class ReadLinesFromSave
{
    public static void Main()
    {
        ReadLines("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup .crp", 45530463);
        Console.WriteLine("\n==================================================\n");
        ReadLines("/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp", 45953494);
    }

    static void ReadLines(string path, int offset)
    {
        Console.WriteLine("Reading lines from: " + Path.GetFileName(path) + " at " + offset);
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            fs.Seek(offset + 16, SeekOrigin.Begin); // Right after "TransportManager"

            // EncodedArray/UInt::BeginRead
            // In ColossalFramework: EncodedArray/UInt has a header byte/flag
            // Let's decode the EncodedArray/UInt::Read implementation
        }
    }
}
