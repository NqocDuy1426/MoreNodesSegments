
using System;
using Mono.Cecil;

class Program {
    static void Main() {
        var asm = AssemblyDefinition.ReadAssembly("/Users/nqocduy/Library/Application Support/Steam/steamapps/common/Cities_Skylines/Cities.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll");
        foreach (var t in asm.MainModule.Types) {
            if (t.BaseType != null && t.BaseType.FullName.Contains("SimulationManagerBase")) {
                bool overridesUpd = false;
                bool overridesLate = false;
                bool overridesEarly = false;
                foreach (var m in t.Methods) {
                    if (m.Name == "UpdateData") overridesUpd = true;
                    if (m.Name == "LateUpdateData") overridesLate = true;
                    if (m.Name == "EarlyUpdateData") overridesEarly = true;
                }
                Console.WriteLine(string.Format("{0} : {1} (Overrides: Update={2}, Late={3}, Early={4})", 
                    t.Name, t.BaseType.FullName, overridesUpd, overridesLate, overridesEarly));
            }
        }
    }
}
