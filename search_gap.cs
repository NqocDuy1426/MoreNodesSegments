using System;
using System.IO;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        using (var fs = File.OpenRead(path))
        {
            fs.Seek(54370268, SeekOrigin.Begin);
            Console.WriteLine("Offset 54370268:");
            
            // Search for "Manager" or other strings in the next 15000 bytes
            byte[] buf = new byte[15000];
            int read = fs.Read(buf, 0, buf.Length);
            
            for (int i = 0; i < read - 5; i++)
            {
                if (buf[i] == 254 && buf[i+1] == 128 && buf[i+2] == 0) // unique string ascii
                {
                    int len = buf[i+3];
                    if (len > 3 && len < 100 && i + 4 + len <= read)
                    {
                        char[] c = new char[len];
                        int running = 128;
                        bool ok = true;
                        for (int k = 0; k < len; k++)
                        {
                            running = (running + buf[i + 4 + k]) & 0xFF;
                            if (running < 32 || running > 126) { ok = false; break; }
                            c[k] = (char)running;
                        }
                        if (ok)
                        {
                            Console.WriteLine(string.Format("Found string at {0}: '{1}'", 54370268 + i, new string(c)));
                        }
                    }
                }
            }
        }
    }
}
