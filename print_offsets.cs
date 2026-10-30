using System;
using ColossalFramework.Packaging;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup3.crp";
        Package pkg = new Package(null, path);
        foreach (Package.Asset a in pkg)
        {
            Console.WriteLine(string.Format("{0}: offset={1}, size={2}", a.name, a.offset, a.size));
        }
    }
}
