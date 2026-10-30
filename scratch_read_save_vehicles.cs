using System;
using System.IO;
using System.Reflection;
using ColossalFramework.IO;
using ColossalFramework.Packaging;

class Program
{
    static void Main()
    {
        string savePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + 
            "/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        if (!File.Exists(savePath))
        {
            savePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + 
                "/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup3.crp";
        }
        Console.WriteLine("Reading save: " + savePath);

        using (FileStream fs = File.OpenRead(savePath))
        {
            Package pkg = PackageHelper.OpenPackage(fs);
            foreach (Package.Asset asset in pkg)
            {
                if (asset.name != null && asset.name.EndsWith("_Data"))
                {
                    Console.WriteLine("Found asset: " + asset.name + " (" + asset.size + " bytes)");
                    using (Stream s = asset.GetStream())
                    {
                        var dsCtor = typeof(DataSerializer).GetConstructor(
                            BindingFlags.NonPublic | BindingFlags.Instance, null,
                            new Type[] { typeof(Stream), typeof(DataSerializer.Mode), typeof(bool) }, null);
                        DataSerializer ds = (DataSerializer)dsCtor.Invoke(new object[] { s, DataSerializer.Mode.File, false });
                        
                        var meta = DataSerializer.Deserialize<SimulationMetaData>(s, DataSerializer.Mode.File);
                        Console.WriteLine("City: " + meta.m_CityName);
                        
                        // Scan serializer stream for VehicleManager tags
                        // In Cities: Skylines, each manager has a tag (e.g. "ColossalOrder.Cities:VehicleManager:3" or similar)
                    }
                }
            }
        }
    }
}

class PackageHelper
{
    public static Package OpenPackage(Stream stream)
    {
        Type pkgType = typeof(Package);
        MethodInfo openM = pkgType.GetMethod("Open", new Type[] { typeof(Stream) });
        return (Package)openM.Invoke(null, new object[] { stream });
    }
}
