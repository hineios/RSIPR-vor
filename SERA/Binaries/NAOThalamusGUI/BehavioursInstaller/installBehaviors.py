import sys
import os

from naoqi import ALProxy

IP = "localhost"  
PORT = 9559


try:
  proxy = ALProxy("ALBehaviorManager", IP, PORT)
except Exception, e:
  print "Error when creating face detection proxy:"
  print str(e)
  exit(1)

for item in os.listdir("./"):
    if item.endswith(".crg"): 
		currentDirectory = os.path.dirname(os.path.abspath(__file__))
		localPath = ""+item.replace(".crg","")
		absolutePath = currentDirectory+"/"+item
		print "proxy.installBehavior("+absolutePath+","+localPath+",False)"
		res = proxy.installBehavior(absolutePath,localPath,False)

		if (res):
			print "Success"
		else:
			print "Fail"

