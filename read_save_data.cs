using System;
using System.IO;
using System.Reflection;
using ColossalFramework.Packaging;

class Program
{
    static void Main()
    {
        string crpPath = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup3.crp";
        Console.WriteLine("Opening " + crpPath);
        Package pkg = PackageHelper.OpenPackage(crpPath);
        if (pkg == null) { Console.WriteLine("Failed to open package"); return; }
        Console.WriteLine("Package: " + pkg.packageName);
        foreach (Package.Asset asset in pkg)
        {
            Console.WriteLine(string.Format("Asset: {0} ({1}) size={2}", asset.name, asset.type, asset.size));
        }
    }
}

static class PackageHelper
{
    public static Package OpenPackage(string path)
    {
        MethodInfo m = typeof(PackageHelper).Assembly.GetType("ColossalFramework.Packaging.PackageDeserializer")
            ?.GetMethod("Deserialize", new Type[] { typeof(string), typeof(Stream) });
        if (m != null)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return m.Invoke(null, new object[] { Path.GetFileNameWithoutExtension(path), fs }) as Package;
            }
        }
        return null;
    }
}
