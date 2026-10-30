using System;
using UnityEngine;

class Program
{
    static void Main()
    {
        float nan = float.NaN;
        float inf = float.PositiveInfinity;
        Console.WriteLine("RoundToInt(NaN): " + Mathf.RoundToInt(nan));
        Console.WriteLine("RoundToInt(Inf): " + Mathf.RoundToInt(inf));
        Console.WriteLine("(int)NaN: " + (int)nan);
        Console.WriteLine("(int)Inf: " + (int)inf);
    }
}
