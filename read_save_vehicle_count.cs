using System;
using System.IO;
using System.Reflection;
using ColossalFramework.IO;

class Program
{
    static void Main()
    {
        string gameManaged = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        Assembly.LoadFrom(gameManaged + "/Assembly-CSharp.dll");
        Assembly.LoadFrom(gameManaged + "/ColossalManaged.dll");

        string savePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup2.crp";
        using (FileStream fs = File.OpenRead(savePath))
        {
            var p = PackageHelper.OpenPackage(fs);
            Console.WriteLine("Package opened successfully.");
            var simAsset = p.Find("Simulation");
            if (simAsset != null)
            {
                Console.WriteLine("Simulation asset found: " + simAsset.size + " bytes");
            }
        }
    }
}

class PackageHelper
{
    public static dynamic OpenPackage(Stream stream)
    {
        Type pkgType = Type.GetType("ColossalFramework.Packaging.Package, ColossalManaged");
        MethodInfo openM = pkgType.GetMethod("Open", new Type[] { typeof(Stream) });
        return openM.Invoke(null, new object[] { stream });
    }
}
