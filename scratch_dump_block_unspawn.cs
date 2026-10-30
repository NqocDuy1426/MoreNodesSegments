using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        string[] types = new string[] { "VehicleAI", "CarAI" };
        foreach (var typeName in types) {
            var t = asm.MainModule.GetType(typeName);
            foreach (var m in t.Methods) {
                if (m.Body != null) {
                    for (int i = 0; i < m.Body.Instructions.Count; i++) {
                        var instr = m.Body.Instructions[i];
                        if (instr.ToString().Contains("m_blockCounter") || instr.ToString().Contains("Unspawn")) {
                            Console.WriteLine(typeName + "." + m.Name + " (" + m.Parameters.Count + "p) at IL_" + instr.Offset.ToString("X4") + ": " + instr);
                            // Print 10 lines of context
                            int s = Math.Max(0, i - 5);
                            int e = Math.Min(m.Body.Instructions.Count - 1, i + 5);
                            for (int j = s; j <= e; j++) {
                                Console.WriteLine("    IL_" + m.Body.Instructions[j].Offset.ToString("X4") + ": " + m.Body.Instructions[j]);
                            }
                            Console.WriteLine("----------------------------------");
                        }
                    }
                }
            }
        }
    }
}
