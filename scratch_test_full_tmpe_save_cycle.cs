using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

class Program
{
    static void Main()
    {
        string dir = "/Users/nqocduy/Library/Application Support/Steam/steamapps/workshop/content/255710/1637663252/";
        Assembly tmpe = Assembly.LoadFrom(dir + "TrafficManager.dll");
        Type ttlCfgType = tmpe.GetType("TrafficManager.Configuration+TimedTrafficLights");

        // 1. Simulate saving
        byte[] payload;
        using (MemoryStream ms = new MemoryStream())
        using (BinaryWriter bw = new BinaryWriter(ms))
        {
            bw.Write((uint)0x4D4E4354); // 'MNCT'
            bw.Write((byte)1);          // version 1

            // Junction restrictions count = 1
            bw.Write((int)1);
            bw.Write((ushort)40000);    // segmentId
            bw.Write(true);             // startNode
            bw.Write((byte)1);          // uturn = False
            bw.Write((byte)2);          // enterBlocked = True
            bw.Write((byte)1);          // pedestrian = False
            bw.Write((byte)0);          // straight = Undefined
            bw.Write((byte)0);          // nearTurnOnRed = Undefined
            bw.Write((byte)0);          // farTurnOnRed = Undefined

            // Priority signs count = 1
            bw.Write((int)1);
            bw.Write((ushort)40000);    // segmentId
            bw.Write(true);             // startNode
            bw.Write((byte)2);          // Stop sign

            // Timed traffic lights count = 1
            Type listType = typeof(List<>).MakeGenericType(ttlCfgType);
            IList ttlList = (IList)Activator.CreateInstance(listType);
            object ttl = Activator.CreateInstance(ttlCfgType);
            ttlCfgType.GetField("nodeId").SetValue(ttl, (ushort)35000);
            ttlCfgType.GetField("started").SetValue(ttl, true);
            ttlList.Add(ttl);

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ttlStream = new MemoryStream())
            {
                bf.Serialize(ttlStream, ttlList);
                byte[] ttlBytes = ttlStream.ToArray();
                bw.Write((int)ttlBytes.Length);
                bw.Write(ttlBytes);
            }

            payload = ms.ToArray();
        }

        Console.WriteLine($"Total payload length: {payload.Length} bytes");

        // 2. Simulate loading
        using (MemoryStream ms = new MemoryStream(payload))
        using (BinaryReader br = new BinaryReader(ms))
        {
            uint magic = br.ReadUInt32();
            byte ver = br.ReadByte();
            Console.WriteLine($"Magic: 0x{magic:X8}, Ver: {ver}");

            int jrCount = br.ReadInt32();
            Console.WriteLine($"Junction Restrictions count: {jrCount}");
            for (int i = 0; i < jrCount; i++)
            {
                ushort segId = br.ReadUInt16();
                bool start = br.ReadBoolean();
                byte uturn = br.ReadByte();
                byte enterBlocked = br.ReadByte();
                byte ped = br.ReadByte();
                byte straight = br.ReadByte();
                byte near = br.ReadByte();
                byte far = br.ReadByte();
                Console.WriteLine($"  JR: seg={segId}, start={start}, uturn={uturn}, enterBlocked={enterBlocked}, ped={ped}");
            }

            int psCount = br.ReadInt32();
            Console.WriteLine($"Priority Signs count: {psCount}");
            for (int i = 0; i < psCount; i++)
            {
                ushort segId = br.ReadUInt16();
                bool start = br.ReadBoolean();
                byte sign = br.ReadByte();
                Console.WriteLine($"  PS: seg={segId}, start={start}, sign={sign}");
            }

            int ttlBytesLen = br.ReadInt32();
            Console.WriteLine($"TTL bytes len: {ttlBytesLen}");
            byte[] ttlBytes = br.ReadBytes(ttlBytesLen);
            using (MemoryStream ttlStream = new MemoryStream(ttlBytes))
            {
                BinaryFormatter bf = new BinaryFormatter();
                IList loadedList = (IList)bf.Deserialize(ttlStream);
                Console.WriteLine($"  TTL count: {loadedList.Count}");
                object first = loadedList[0];
                Console.WriteLine($"  TTL[0].nodeId = {ttlCfgType.GetField("nodeId").GetValue(first)}");
            }
        }
    }
}
