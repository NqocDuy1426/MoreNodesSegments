with open("scratch_reproduce_crash.cs", "r") as f:
    text = f.read()

target = 'sm.m_pauseOnLoad = new SavedBool("pauseOnLoad", "gameSettings", false);'
replacement = 'typeof(SimulationManager).GetField("m_pauseOnLoad", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(sm, new SavedBool("pauseOnLoad", "gameSettings", false));'
text = text.replace(target, replacement)

with open("scratch_reproduce_crash.cs", "w") as f:
    f.write(text)
print("Updated scratch_reproduce_crash.cs")
