using System;

class Program
{
    static void Main()
    {
        for (ushort nodeID = 30000; nodeID <= 40000; nodeID += 5000)
        {
            uint nodeFrameIndex = (uint)((nodeID << 8) / 32768);
            Console.WriteLine($"nodeID: {nodeID}, nodeFrameIndex: {nodeFrameIndex}, (nodeFrameIndex >> 8): {nodeFrameIndex >> 8}, (nodeFrameIndex & 255): {nodeFrameIndex & 255}");

            for (uint frame = 1000; frame < 1005; frame++)
            {
                uint simFrame = frame;
                int simBuffer = (int)((simFrame >> 8) & 1);

                uint carFrame = frame - nodeFrameIndex;
                int carBuffer = (int)((carFrame >> 8) & 1);

                Console.WriteLine($"   frame={frame}: Sim writes to buf {simBuffer}, Car/Visual reads buf {carBuffer} (MATCH: {simBuffer == carBuffer})");
            }
        }
    }
}
