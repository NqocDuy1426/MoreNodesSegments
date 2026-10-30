using System;
using System.IO;
using ColossalFramework.Packaging;

class Program
{
    static void Main()
    {
        string dir = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Application Support/Colossal Order/Cities_Skylines/Saves";
        string file = dir + "/QuangNinh backup2.crp";
        var pkg = new Package("QuangNinh backup2", file);
        foreach (Package.Asset a in pkg)
        {
            Console.WriteLine(string.Format("Asset: '{0}' type: '{1}'", a.name, a.type));
        }
    }
}
