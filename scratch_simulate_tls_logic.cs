using System;

class Program
{
    static void Main()
    {
        // Let's trace the local variables of TrafficLightSimulationStep
        // clock: V_2, step: V_3, pedStep: V_4
        // V_5: current green has vehicles
        // V_6: current green segment index
        // V_7: first red segment with vehicles
        // V_8: first red segment with peds
        // V_9: selected red segment with vehicles for step
        // V_10: selected red segment with peds for step

        Console.WriteLine("Simulating TrafficLightSimulationStep with 4 segments (cross intersection)...");
        // Suppose segment 0 is currently Green, segments 1, 2, 3 are Red.
        // Let's test two scenarios:
        // Case A: Cars waiting on segment 1 (vehicles = true)
        // Case B: No cars waiting anywhere (vehicles = false everywhere)

        Simulate(hasVehiclesOnGreen: true, hasVehiclesOnRed: true);
        Simulate(hasVehiclesOnGreen: false, hasVehiclesOnRed: true);
        Simulate(hasVehiclesOnGreen: false, hasVehiclesOnRed: false);
    }

    static void Simulate(bool hasVehiclesOnGreen, bool hasVehiclesOnRed)
    {
        Console.WriteLine($"\n--- Scenario: GreenHasVehicles={hasVehiclesOnGreen}, RedHasVehicles={hasVehiclesOnRed} ---");
        int clock = 0;
        int step = 0;
        int currentGreen = 0; // segment 0

        for (int frame = 0; frame < 5; frame++)
        {
            int V_5 = -1;
            int V_6 = -1;
            int V_7 = -1;
            int V_8 = -1;
            int V_9 = -1;
            int V_10 = -1;
            int V_13 = 0;

            for (int seg = 0; seg < 4; seg++)
            {
                bool isGreen = (seg == currentGreen);
                bool hasVehicles = isGreen ? hasVehiclesOnGreen : hasVehiclesOnRed;

                if (isGreen)
                {
                    V_6 = seg;
                    if (hasVehicles) V_5 = seg;
                }
                else
                {
                    if (hasVehicles)
                    {
                        if (V_7 == -1) V_7 = seg;
                        if (V_9 == -1 && V_13 >= step) V_9 = seg;
                    }
                }
                V_13++;
            }

            if (V_9 == -1) V_9 = V_7;
            if (V_10 == -1) V_10 = V_8;

            // IL_01f5:
            bool willChange = false;
            if (V_6 != -1 && V_5 != -1 && clock <= 1)
            {
                // Green has vehicles and clock <= 1 -> don't change
                V_9 = -1;
                V_10 = -1;
                clock++;
            }
            else
            {
                // Change!
                if (V_9 != -1)
                {
                    V_6 = V_9;
                    willChange = true;
                }
            }

            Console.WriteLine($"Frame {frame}: clock={clock}, step={step}, currentGreen={currentGreen}, nextGreen={V_6}, changed={willChange}");
            if (willChange)
            {
                currentGreen = V_6;
                step = (step + 1) % 4;
                clock = 0;
            }
        }
    }
}
