import subprocess
import os
import shutil
import sys

hostAddress = sys.argv[1]

newpath = ".\\tmp"
if os.path.exists(newpath):
    shutil.rmtree(newpath)
os.makedirs(newpath)

swaggerUrl = hostAddress + "swagger/v1/swagger.json"
print(f'Reading Swagger Doc from {swaggerUrl}')

# subprocess.run(
#     f'java -jar swagger-codegen-cli.jar generate \
#         -i {swaggerUrl} \
#         -l android --library=volley \
#         -c SwaggerConfigAndroid.json \
#         -o .\\tmp\\', shell=True, check=True)
subprocess.run(
    f'java -jar swagger-codegen-cli.jar generate \
        -i {swaggerUrl} \
        -l java --library=okhttp-gson \
        -c SwaggerConfigJava.json \
        -o .\\tmp\\', shell=True, check=True)
subprocess.run("gradle build -p .\\tmp", shell=True, check=True)

jarsource = ".\\tmp\\build\\libs\\swagger-java-client-1.0.0.jar"
jartarget = "..\\app\\libs\\"
os.makedirs(jartarget, exist_ok=True)
jarname = "climb-client.jar"

if os.path.exists(jartarget + jarname):
    os.remove(jartarget + jarname)

os.rename(jarsource, jartarget + jarname)

shutil.rmtree(newpath)
