using System;
using System.IO;

class Program {
    static void Main() {
        var lines = File.ReadAllLines("carai_simstep_6p_il.txt");
        for (int i = 0; i < lines.Length; i++) {
            var l = lines[i];
            if (l.Contains("call ") || l.Contains("callvirt ")) {
                Console.WriteLine(i.ToString("D4") + ": " + l.Trim());
            }
        }
    }
}
