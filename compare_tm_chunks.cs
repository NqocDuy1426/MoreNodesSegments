using System;
using System.IO;

public class CompareTmChunks
{
    public static void Main()
    {
        string p1 = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup .crp";
        string p2 = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";

        int off1 = 45530463;
        int len1 = 45572549 - off1;

        int off2 = 45953494;
        int len2 = 45995702 - off2;

        byte[] b1 = new byte[len1];
        byte[] b2 = new byte[len2];

        using (var fs1 = new FileStream(p1, FileMode.Open, FileAccess.Read))
        {
            fs1.Seek(off1, SeekOrigin.Begin);
            fs1.Read(b1, 0, len1);
        }

        using (var fs2 = new FileStream(p2, FileMode.Open, FileAccess.Read))
        {
            fs2.Seek(off2, SeekOrigin.Begin);
            fs2.Read(b2, 0, len2);
        }

        Console.WriteLine(string.Format("Save 1 TM Chunk: {0} bytes", b1.Length));
        Console.WriteLine(string.Format("Save 2 TM Chunk: {0} bytes", b2.Length));

        // Find common prefix
        int samePrefix = 0;
        while (samePrefix < Math.Min(b1.Length, b2.Length) && b1[samePrefix] == b2[samePrefix])
        {
            samePrefix++;
        }
        Console.WriteLine(string.Format("Common prefix: {0} bytes", samePrefix));

        // Let's print the bytes where they diverge
        Console.WriteLine("\nBytes at divergence (Save 1 vs Save 2):");
        for (int i = 0; i < 32 && samePrefix + i < Math.Min(b1.Length, b2.Length); i++)
        {
            Console.WriteLine(string.Format("  +{0,3} (0x{1:X4}): S1={2:X2} vs S2={3:X2}",
                i, samePrefix + i, b1[samePrefix + i], b2[samePrefix + i]));
        }
    }
}
