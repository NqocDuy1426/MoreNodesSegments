with open("scratch_reproduce_crash.cs", "r") as f:
    text = f.read()

target = 'SetSingleton(typeof(SimulationManager), sm);'
replacement = target + '\n        var gs = CreateInstance(typeof(GameSettings));\n        SetSingleton(typeof(GameSettings), gs);'
text = text.replace(target, replacement)

with open("scratch_reproduce_crash.cs", "w") as f:
    f.write(text)
print("Updated scratch_reproduce_crash.cs with GameSettings")
