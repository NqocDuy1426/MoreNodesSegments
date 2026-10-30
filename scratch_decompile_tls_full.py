import re

with open("tls_instructions.txt") as f:
    lines = f.readlines()

for line in lines:
    print(line.rstrip())
