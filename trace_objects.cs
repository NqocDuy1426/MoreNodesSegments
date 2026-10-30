using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using ColossalFramework.IO;

class Program {
    static void Main() {
        string file = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        using (var fs = File.OpenRead(file)) {
            fs.Seek(657, SeekOrigin.Begin);
            
            // Create DataSerializer
            var ctor = typeof(DataSerializer).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Stream), typeof(DataSerializer.Mode), typeof(bool) }, null);
            var ds = (DataSerializer)ctor.Invoke(new object[] { fs, DataSerializer.Mode.File, false });
            
            // In DataSerializer.Deserialize, what happens first?
            // Let's inspect the fields initialized in DataSerializer
            Console.WriteLine("DataSerializer created! fs.Position: " + fs.Position);
            
            // Let's see: DataSerializer reads header: version, etc.
            // Let's inspect DataSerializer methods:
            var mInit = typeof(DataSerializer).GetMethod("DeserializeImplementation", BindingFlags.NonPublic | BindingFlags.Instance);
            if (mInit != null) {
                Console.WriteLine("Found DeserializeImplementation");
            }
        }
    }
}
