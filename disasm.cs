using System;
using System.Reflection;
using System.Reflection.Emit;

class Program {
    static void Main() {
        var asm = Assembly.LoadFrom("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        var m = typeof(LoadingProfiler).GetMethod("EndDeserialize", BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(ColossalFramework.IO.DataSerializer), typeof(string) }, null);
        var mb = m.GetMethodBody();
        byte[] il = mb.GetILAsByteArray();
        int pos = 0;
        var module = m.Module;
        while (pos < il.Length) {
            int opPos = pos;
            byte b = il[pos++];
            OpCode op = (b == 0xFE) ? typeof(OpCodes).GetFields()[0].GetValue(null) as OpCode : null; // simplified
            // let's print byte and offset
            Console.WriteLine(string.Format("{0:X4}: {1:X2}", opPos, b));
        }
    }
}
