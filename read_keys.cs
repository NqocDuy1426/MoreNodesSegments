using System;
using System.IO;
using System.Text;
using ColossalFramework.Packaging;

class Program
{
    static void Main()
    {
        string path = "/Users/nqocduy/Library/Application Support/Colossal Order/Cities_Skylines/Saves/QuangNinh backup3.crp";
        Package pkg = new Package(null, path);
        Package.Asset dataAsset = null;
        foreach (Package.Asset a in pkg)
        {
            if (a.name.EndsWith("_Data")) { dataAsset = a; break; }
        }
        if (dataAsset == null) return;
        byte[] bytes = dataAsset.GetBytes();
        Console.WriteLine("DataAsset bytes: " + bytes.Length);

        string[] targets = new string[] {
            "MoreNetworkCapacity_HighID_TMPE_v1",
            "MoreNetworkCapacity_HighID_TMPE_v2",
            "TrafficManager_TimedLights",
            "TrafficManager_CustomSegmentLights",
            "TrafficManager_SpeedLimits",
            "TrafficManager_LaneConnections"
        };

        foreach (var t in targets)
        {
            byte[] needle = Encoding.UTF8.GetBytes(t);
            int idx = IndexOf(bytes, needle);
            Console.WriteLine(string.Format("Key '{0}': found at {1}", t, idx));
        }
    }

    static int IndexOf(byte[] haystack, byte[] needle)
    {
        for (int i = 0; i <= haystack.Length - needle.Length; i++)
        {
            bool match = true;
            for (int j = 0; j < needle.Length; j++)
            {
                if (haystack[i + j] != needle[j]) { match = false; break; }
            }
            if (match) return i;
        }
        return -1;
    }
}
