using System;
using System.IO;

public class DiffTmFull
{
    public static void Main()
    {
        string p1 = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup .crp";
        string p2 = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";

        byte[] b1 = new byte[42086];
        byte[] b2 = new byte[42208];

        using (var fs1 = new FileStream(p1, FileMode.Open, FileAccess.Read))
        {
            fs1.Seek(45530463, SeekOrigin.Begin);
            fs1.Read(b1, 0, b1.Length);
        }

        using (var fs2 = new FileStream(p2, FileMode.Open, FileAccess.Read))
        {
            fs2.Seek(45953494, SeekOrigin.Begin);
            fs2.Read(b2, 0, b2.Length);
        }

        Console.WriteLine(string.Format("Save 1: {0} bytes, Save 2: {1} bytes", b1.Length, b2.Length));

        // Find how many differences
        int diffs = 0;
        int minLen = Math.Min(b1.Length, b2.Length);
        for (int i = 0; i < minLen; i++)
        {
            if (b1[i] != b2[i]) diffs++;
        }
        Console.WriteLine(string.Format("Differing bytes in common length: {0} / {1} ({2:F1}%)",
            diffs, minLen, (double)diffs / minLen * 100));
    }
}
