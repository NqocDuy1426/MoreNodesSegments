with open("scratch_reproduce_crash.cs", "r") as f:
    text = f.read()

target = 'SetSingleton(typeof(GameSettings), gs);'
replacement = """var dict = new Dictionary<string, SettingsFile>();
        var sf = (SettingsFile)CreateInstance(typeof(SettingsFile));
        dict["gameSettings"] = sf;
        typeof(GameSettings).GetField("m_SettingsFiles", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(gs, dict);
        SetSingleton(typeof(GameSettings), gs);"""
text = text.replace(target, replacement)

with open("scratch_reproduce_crash.cs", "w") as f:
    f.write(text)
print("Updated scratch_reproduce_crash.cs with SettingsFiles")
