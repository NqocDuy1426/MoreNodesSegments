using System;
using System.IO;
using System.Collections.Generic;

class Program {
    static void Main() {
        using (var fs = File.OpenRead("backup4_sim.bin")) {
            Console.WriteLine("Opening backup4_sim.bin, length=" + fs.Length);
            // Search for NetManager+Data
            // We know NetManager serialized data is in the stream.
            // Let's find the position where EncodedArray or NetManager occurs!
            byte[] buf = File.ReadAllBytes("backup4_sim.bin");
            string s = System.Text.Encoding.ASCII.GetString(buf);
            int idx = s.IndexOf("NetManager");
            while (idx >= 0) {
                Console.WriteLine("Found 'NetManager' at offset " + idx);
                idx = s.IndexOf("NetManager", idx + 1);
            }
        }
    }
}
