using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        string tmpeDll = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/TrafficManager.dll";
        string gameDir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed";
        
        Assembly.LoadFrom(Path.Combine(gameDir, "Assembly-CSharp.dll"));
        Assembly.LoadFrom(Path.Combine(gameDir, "ColossalManaged.dll"));
        Assembly.LoadFrom(Path.Combine(gameDir, "ICities.dll"));
        Assembly.LoadFrom(Path.Combine(gameDir, "UnityEngine.dll"));
        Assembly tmpe = Assembly.LoadFrom(tmpeDll);

        // Test payload v2 structure
        using (MemoryStream ms = new MemoryStream())
        using (BinaryWriter bw = new BinaryWriter(ms))
        {
            bw.Write((uint)0x4D4E4354); // Magic
            bw.Write((byte)2);          // Version 2

            // Section 1: Junction Restrictions
            bw.Write((int)1); // 1 record
            bw.Write((ushort)40000); // segId
            bw.Write(true); // startNode
            bw.Write((byte)1); // uturn
            bw.Write((byte)1); // blocked
            bw.Write((byte)1); // ped
            bw.Write((byte)1); // straight
            bw.Write((byte)1); // near
            bw.Write((byte)1); // far

            // Section 2: Priority Signs
            bw.Write((int)1); // 1 record
            bw.Write((ushort)40000);
            bw.Write(true);
            bw.Write((byte)2); // Stop

            // Section 3: Timed Traffic Lights
            bw.Write((int)0); // ttlLen 0 for test

            // Section 4: Lane Connections
            bw.Write((int)1); // 1 connection
            bw.Write((uint)270000); // lowerLaneId
            bw.Write((uint)270001); // higherLaneId
            bw.Write(true); // lowerStartNode
            bw.Write((int)0); // group

            // Section 5: Lane Speed Limits
            bw.Write((int)1); // 1 limit
            bw.Write((uint)270000);
            bw.Write(1.25f);

            // Section 6: Lane Arrows
            bw.Write((int)1); // 1 arrow
            bw.Write((uint)270000);
            bw.Write((uint)3); // forward + left

            // Section 7: Vehicle Restrictions
            bw.Write((int)1);
            bw.Write((uint)270000);
            bw.Write((uint)15); // car, bus, etc.

            // Section 8: Parking Restrictions
            bw.Write((int)1);
            bw.Write((ushort)40000);
            bw.Write(false); // forward
            bw.Write(true);  // backward

            byte[] payload = ms.ToArray();
            Console.WriteLine("Payload v2 test byte length: " + payload.Length);

            // Verify reader
            using (MemoryStream rMs = new MemoryStream(payload))
            using (BinaryReader br = new BinaryReader(rMs))
            {
                uint magic = br.ReadUInt32();
                byte ver = br.ReadByte();
                Console.WriteLine("Magic: " + magic.ToString("X") + " Ver: " + ver);
                int jrCount = br.ReadInt32();
                ushort seg = br.ReadUInt16();
                bool sn = br.ReadBoolean();
                byte ut = br.ReadByte(), bl = br.ReadByte(), pd = br.ReadByte(), st = br.ReadByte(), nr = br.ReadByte(), fr = br.ReadByte();
                Console.WriteLine("JR Seg: " + seg + " sn: " + sn);

                int psCount = br.ReadInt32();
                ushort psSeg = br.ReadUInt16();
                bool psSn = br.ReadBoolean();
                byte psSign = br.ReadByte();
                Console.WriteLine("PS Seg: " + psSeg + " Sign: " + psSign);

                int ttlLen = br.ReadInt32();
                Console.WriteLine("TTL Len: " + ttlLen);

                int lcCount = br.ReadInt32();
                uint l1 = br.ReadUInt32(), l2 = br.ReadUInt32();
                bool lStart = br.ReadBoolean();
                int lGroup = br.ReadInt32();
                Console.WriteLine("LC: " + l1 + " -> " + l2);

                int slCount = br.ReadInt32();
                uint slLane = br.ReadUInt32();
                float speed = br.ReadSingle();
                Console.WriteLine("Speed: lane " + slLane + " speed " + speed);

                int laCount = br.ReadInt32();
                uint laLane = br.ReadUInt32();
                uint laFlags = br.ReadUInt32();
                Console.WriteLine("Arrows: lane " + laLane + " flags " + laFlags);

                int vrCount = br.ReadInt32();
                uint vrLane = br.ReadUInt32();
                uint vrTypes = br.ReadUInt32();
                Console.WriteLine("VehRestrictions: lane " + vrLane + " types " + vrTypes);

                int prCount = br.ReadInt32();
                ushort prSeg = br.ReadUInt16();
                bool fwd = br.ReadBoolean(), bwd = br.ReadBoolean();
                Console.WriteLine("Parking: seg " + prSeg + " fwd " + fwd + " bwd " + bwd);
            }
        }
        Console.WriteLine("Payload v2 test SUCCESS!");
    }
}
