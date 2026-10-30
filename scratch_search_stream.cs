using System;
using System.IO;
using System.Text;
using ColossalFramework.Packaging;

class Program
{
    static void Main()
    {
        string p = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        using (var fs = File.OpenRead(p))
        {
            var pkg = Package.Open(fs);
            foreach (var asset in pkg)
            {
                if (asset.name != null && asset.name.EndsWith("_Data"))
                {
                    Console.WriteLine("Data asset: " + asset.name + ", size: " + asset.size);
                    using (var s = asset.GetStream())
                    {
                        byte[] buf = new byte[asset.size];
                        int total = 0;
                        while (total < buf.Length)
                        {
                            int read = s.Read(buf, total, buf.Length - total);
                            if (read <= 0) break;
                            total += read;
                        }
                        Console.WriteLine("Decompressed stream bytes read: " + total);
                        
                        string[] search = { "VehicleManager", "PathManager", "NetManager", "CitizenManager" };
                        foreach (var target in search)
                        {
                            byte[] b = Encoding.UTF8.GetBytes(target);
                            for (int i = 0; i < total - b.Length; i++)
                            {
                                bool match = true;
                                for (int j = 0; j < b.Length; j++)
                                {
                                    if (buf[i + j] != b[j]) { match = false; break; }
                                }
                                if (match)
                                {
                                    Console.WriteLine("Found '" + target + "' in stream at offset: " + i);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
