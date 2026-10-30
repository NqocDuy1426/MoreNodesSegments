using System;
using System.IO;
using ColossalFramework.Packaging;

class Program
{
    static void Main(string[] args)
    {
        string p1 = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        string p2 = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup3.crp";

        Inspect(p1);
        Inspect(p2);
    }

    static void Inspect(string path)
    {
        Console.WriteLine("=== INSPECTING: " + Path.GetFileName(path) + " ===");
        var pkg = Package.Open(path);
        Console.WriteLine("Package Name: " + pkg.packageName + " Version: " + pkg.version);
        foreach (var asset in pkg.FilterAssets(new Package.AssetType[0]))
        {
            Console.WriteLine("Asset: " + asset.name + " (" + asset.type + ") size: " + asset.size);
        }
    }
}
