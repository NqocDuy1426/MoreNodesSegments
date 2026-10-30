with open("scratch_reproduce_crash.cs", "r") as f:
    text = f.read()

text = text.replace('"GameAreaManager",', '')

with open("scratch_reproduce_crash.cs", "w") as f:
    f.write(text)
print("Updated scratch_reproduce_crash.cs removed GameAreaManager from loop")
