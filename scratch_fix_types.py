with open("scratch_test_compile_serializer.cs") as f:
    text = f.read()

text = text.replace("Type commons = GetTMPECommonsAssembly();", "Assembly commons = GetTMPECommonsAssembly();")
text = text.replace("Type apiAsm = GetTMPEAPIAssembly();", "Assembly apiAsm = GetTMPEAPIAssembly();")

with open("scratch_test_compile_serializer.cs", "w") as f:
    f.write(text)
