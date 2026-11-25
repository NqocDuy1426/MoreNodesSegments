
using System;
using System.IO;
using ColossalFramework.Packaging;

class Program
{
    static void Main()
    {
        try
        {
            string savePath = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/backup12.crp";
            using (var fs = File.OpenRead(savePath))
            {
                var pkg = Package.Deserialize(fs);
                Console.WriteLine("Package name: " + pkg.packageName);
                foreach (var asset in pkg)
                {
                    Console.WriteLine(string.Format("  Asset: {0} ({1}), size={2}, offset={3}", asset.name, asset.type, asset.size, asset.offset));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex);
        }
    }
}
