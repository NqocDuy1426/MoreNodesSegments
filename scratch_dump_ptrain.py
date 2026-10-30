import subprocess

out = subprocess.check_output(["mono", "scratch_dump_ticket.exe"]).decode("utf-8")
blocks = out.split("=== ")
for b in blocks:
    if "PassengerTrainAI" in b:
        print("=== " + b)
