
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
            var pkg = new Package("backup12", savePath, true);
            foreach (Package.Asset asset in pkg)
            {
                Console.WriteLine(string.Format("  Asset: name={0}, type={1}, size={2}, offset={3}", asset.name, asset.type, asset.size, asset.offset));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex);
        }
    }
}
