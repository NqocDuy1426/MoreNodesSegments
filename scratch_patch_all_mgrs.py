with open("scratch_reproduce_crash.cs", "r") as f:
    text = f.read()

target = 'var asm = typeof(NetManager).Assembly;'
replacement = """var asm = typeof(NetManager).Assembly;
        foreach (var t in asm.GetTypes())
        {
            if (t.IsClass && !t.IsAbstract && (t.Name.EndsWith("Manager") || t.Name.EndsWith("Simulation") || t.Name.EndsWith("System")))
            {
                try
                {
                    var baseType = t.BaseType;
                    while (baseType != null)
                    {
                        if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(Singleton<>))
                        {
                            SetSingleton(t, CreateInstance(t));
                            break;
                        }
                        baseType = baseType.BaseType;
                    }
                }
                catch {}
            }
        }"""
text = text.replace(target, replacement)

with open("scratch_reproduce_crash.cs", "w") as f:
    f.write(text)
print("Updated scratch_reproduce_crash.cs with all managers")
