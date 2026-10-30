with open("scratch_reproduce_crash.cs", "r") as f:
    text = f.read()

target = 'var sf = (SettingsFile)CreateInstance(typeof(SettingsFile));'
replacement = 'var sf = new SettingsFile();'
text = text.replace(target, replacement)

with open("scratch_reproduce_crash.cs", "w") as f:
    f.write(text)
print("Updated scratch_reproduce_crash.cs with new SettingsFile()")
