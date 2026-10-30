with open("scratch_reproduce_crash.cs", "r") as f:
    text = f.read()

# Replace GamAreaManager deserialization error by initializing GameAreaManager fields
target = 'var asm = typeof(NetManager).Assembly;'
replacement = """var gam = (GameAreaManager)CreateInstance(typeof(GameAreaManager));
        gam.m_areaGrid = new int[25];
        SetSingleton(typeof(GameAreaManager), gam);
        var asm = typeof(NetManager).Assembly;"""
text = text.replace(target, replacement)

with open("scratch_reproduce_crash.cs", "w") as f:
    f.write(text)
print("Updated scratch_reproduce_crash.cs with gam.m_areaGrid")
