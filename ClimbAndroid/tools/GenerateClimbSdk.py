import subprocess
import os
import shutil

newpath = ".\\tmp"
if os.path.exists(newpath):
    shutil.rmtree(newpath)
os.makedirs(newpath)

subprocess.run("java -jar swagger-codegen-cli.jar generate -i http://192.168.196.1:45455/swagger/v1/swagger.json -l android --library=volley -o .\\tmp\\", shell=True, check=True)
subprocess.run("gradle build -p .\\tmp", shell=True, check=True)

jarsource = ".\\tmp\\build\\outputs\\jar\\tmp-release-1.0.0.jar"
jartarget = "..\\app\\libs\\"
os.makedirs(jartarget, exist_ok=True)
jarname = "climb.jar"

os.remove(jartarget + jarname)
os.rename(jarsource, jartarget + jarname)

shutil.rmtree(newpath)
