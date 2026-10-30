using System;
using System.IO;
using System.Reflection;
using ColossalFramework.IO;
using ColossalFramework.Packaging;

class Program
{
    static void Main()
    {
        string p = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        using (FileStream fs = File.OpenRead(p))
        {
            var pkg = new Package(null, p);
            var asset = pkg.Find("QuangNinh backup2_Data");
            if (asset == null)
            {
                foreach (Package.Asset a in pkg)
                {
                    if (a.name != null && a.name.EndsWith("_Data")) { asset = a; break; }
                }
            }
            Console.WriteLine("Data asset: " + asset.name + ", size: " + asset.size);
            using (Stream s = asset.GetStream())
            {
                // In Cities: Skylines DataSerializer:
                // Let us search for the string "VehicleManager" in the stream
                byte[] target = System.Text.Encoding.UTF8.GetBytes("VehicleManager");
                byte[] buf = new byte[asset.size];
                s.Read(buf, 0, buf.Length);
                Console.WriteLine("Read buffer into memory.");

                for (int i = 0; i < buf.Length - target.Length; i++)
                {
                    bool match = true;
                    for (int j = 0; j < target.Length; j++)
                    {
                        if (buf[i + j] != target[j]) { match = false; break; }
                    }
                    if (match)
                    {
                        Console.WriteLine("Found 'VehicleManager' at offset: " + i);
                        // Let us see the bytes around this offset
                        int start = Math.Max(0, i - 20);
                        int end = Math.Min(buf.Length, i + 100);
                        for (int k = start; k < end; k++)
                        {
                            char c = (buf[k] >= 32 && buf[k] <= 126) ? (char)buf[k] : '.';
                            Console.Write(c);
                        }
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}
